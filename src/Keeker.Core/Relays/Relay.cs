using Keeker.Core.Conf;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Keeker.Core.Relays
{
    public class Relay
    {
        private enum ClientFlowState
        {
            Unknown = 0,
            Idle,
            ReadingMetadata,
            RedirectingFixedSizeContent,
        }

        private enum ServerFlowState
        {
            Unknown = 0,
            Idle,
            ReadingMetadata,
            RedirectingFixedSizeContent,
            RedirectingChunkedContent,
        }

        private readonly HostPlainConf _conf;

        private readonly KeekStream _clientStream;
        private ClientFlowState _clientFlowState;
        private readonly AutoBuffer _clientBuffer;
        private readonly FixedSizeContentRedirector _clientFixedSizeContentRedirector;
        private readonly ChunkedContentRedirector _clientChunkedContentRedirector;

        private HttpRequestMetadata _lastRequestMetadata;

        private readonly KeekStream _serverStream;
        private ServerFlowState _serverFlowState;
        private readonly AutoBuffer _serverBuffer;
        private readonly FixedSizeContentRedirector _serverFixedSizeContentRedirector;
        private readonly ChunkedContentRedirector _serverChunkedContentRedirector;

        private HttpResponseMetadata _lastResponseMetadata;

        private readonly string _protocol;
        private readonly string _domesticAuthority;
        private readonly string _domesticAuthorityWithPort;

        public Relay(Stream innerClientStream, string listenerId, string id, HostPlainConf conf)
        {
            this.ListenerId = listenerId;
            this.Id = id;
            _conf = (conf ?? throw new ArgumentNullException(nameof(conf))).Clone();

            // streams
            _clientStream = new KeekStream(innerClientStream);
            _clientBuffer = new AutoBuffer();

            var tcpclient = new TcpClient();
            tcpclient.Connect(_conf.EndPoint);
            var serverNetworkStream = tcpclient.GetStream();

            _serverStream = new KeekStream(serverNetworkStream);
            _serverBuffer = new AutoBuffer();

            // redirectors
            _clientFixedSizeContentRedirector = new FixedSizeContentRedirector(_clientStream, _serverStream, _clientBuffer);
            _clientChunkedContentRedirector = new ChunkedContentRedirector(_clientStream, _serverStream, _clientBuffer);

            _serverFixedSizeContentRedirector = new FixedSizeContentRedirector(_serverStream, _clientStream, _serverBuffer);
            _serverChunkedContentRedirector = new ChunkedContentRedirector(_serverStream, _clientStream, _serverBuffer);

            _protocol = this.IsSecure ? "https" : "http";
            var hostName = _conf.DomesticHostName;
            var defaultPort = this.IsSecure ? 443 : 80;
            string colonWithPortIfNeeded;
            var port = _conf.EndPoint.Port;
            if (defaultPort == port)
            {
                colonWithPortIfNeeded = "";
            }
            else
            {
                colonWithPortIfNeeded = $":{_conf.EndPoint.Port}";
            }

            _domesticAuthority = $"{hostName}{colonWithPortIfNeeded}";
            _domesticAuthorityWithPort = $"{hostName}:{port}";
        }

        public string ListenerId { get; }

        public string Id { get; }

        public void Start()
        {
            new Task(this.ClientRoutine).Start();
            new Task(this.ServerRoutine).Start();
        }

        private void ClientRoutine()
        {
            const int IDLE_TIMEOUT = 1;
            var idleTimeout = TimeSpan.FromMilliseconds(IDLE_TIMEOUT);
            const int PORTION_SIZE = 1000;

            _clientFlowState = ClientFlowState.Idle;;
            var contentBytesCount = 0;

            while (true)
            {
                if (_clientFlowState == ClientFlowState.Idle)
                {
                    if (_clientStream.AccumulatedBytesCount > 0)
                    {
                        _clientFlowState = ClientFlowState.ReadingMetadata;
                        continue;
                    }

                    var bytesReadCount = _clientStream.ReadInnerStream(PORTION_SIZE);

                    if (bytesReadCount == 0)
                    {
                        Thread.Sleep(idleTimeout);
                    }
                    else
                    {
                        _clientFlowState = ClientFlowState.ReadingMetadata;
                    }
                }
                else if (_clientFlowState == ClientFlowState.ReadingMetadata)
                {
                    if (_clientStream.AccumulatedBytesCount == 0)
                    {
                        // let's accumulate some data
                        var bytesReadCount = _clientStream.ReadInnerStream(PORTION_SIZE);

                        if (bytesReadCount == 0)
                        {
                            _clientFlowState = ClientFlowState.Idle;
                            continue;
                        }
                    }

                    var index = _clientStream.PeekIndexOf(HttpHelper.CrLfCrLfBytes);
                    if (index >= 0)
                    {
                        var metadataLength = index + HttpHelper.CrLfCrLfBytes.Length;

                        _clientBuffer.Allocate(metadataLength);
                        _clientStream.Read(_clientBuffer.Buffer, 0, metadataLength);

                        var requestMetadata = HttpRequestMetadata.Parse(_clientBuffer.Buffer, 0);
                        _lastRequestMetadata = requestMetadata;
                        var transformedRequestMetadata = this.TransformRequestMetadata(requestMetadata);
                        var transformedRequestMetadataBytes = transformedRequestMetadata.ToArray();
                        _serverStream.Write(transformedRequestMetadataBytes, 0, transformedRequestMetadataBytes.Length);

                        if (transformedRequestMetadata.Headers.ContainsName("Content-Length"))
                        {
                            _clientFlowState = ClientFlowState.RedirectingFixedSizeContent;
                            contentBytesCount = transformedRequestMetadata.Headers.GetContentLength();
                        }
                    }
                    else
                    {
                        // accumulated data doesn't contain \r\n, read more.
                        _clientStream.ReadInnerStream(PORTION_SIZE);
                    }
                }
                else if (_clientFlowState == ClientFlowState.RedirectingFixedSizeContent)
                {
                    //this.RedirectClientContent(contentBytesCount);
                    _clientFixedSizeContentRedirector.Redirect(contentBytesCount);
                    contentBytesCount = 0;
                    _clientFlowState = ClientFlowState.Idle;
                }
                else
                {
                    throw new ApplicationException(); // todo2[ak] wtf
                }
            }
        }

        private void ServerRoutine()
        {
            const int IDLE_TIMEOUT = 1;
            var idleTimeout = TimeSpan.FromMilliseconds(IDLE_TIMEOUT);
            const int PORTION_SIZE = 1000;

            _serverFlowState = ServerFlowState.Idle;
            var contentBytesCount = 0;

            while (true)
            {
                if (_serverFlowState == ServerFlowState.Idle)
                {
                    if (_serverStream.AccumulatedBytesCount > 0)
                    {
                        _serverFlowState = ServerFlowState.ReadingMetadata;
                        continue;
                    }

                    var bytesReadCount = _serverStream.ReadInnerStream(PORTION_SIZE);

                    if (bytesReadCount == 0)
                    {
                        Thread.Sleep(idleTimeout);
                    }
                    else
                    {
                        _serverFlowState = ServerFlowState.ReadingMetadata;
                    }
                }
                else if (_serverFlowState == ServerFlowState.ReadingMetadata)
                {
                    if (_serverStream.AccumulatedBytesCount == 0)
                    {
                        // let's accumulate some data
                        var bytesReadCount = _serverStream.ReadInnerStream(PORTION_SIZE);

                        if (bytesReadCount == 0)
                        {
                            _serverFlowState = ServerFlowState.Idle;
                            continue;
                        }
                    }

                    var index = _serverStream.PeekIndexOf(HttpHelper.CrLfCrLfBytes);
                    if (index > 0)
                    {
                        var metadataLength = index + HttpHelper.CrLfCrLfBytes.Length;
                        _serverBuffer.Allocate(metadataLength);

                        _serverStream.Read(_serverBuffer.Buffer, 0, metadataLength);

                        var responseMetadata = HttpResponseMetadata.Parse(_serverBuffer.Buffer, 0);
                        _lastResponseMetadata = responseMetadata;

                        var transformedResponseMetadata = this.TransformResponseMetadata(responseMetadata);
                        var transformedResponseMetadataBytes = transformedResponseMetadata.ToArray();
                        _clientStream.Write(transformedResponseMetadataBytes, 0, transformedResponseMetadataBytes.Length);

                        if (transformedResponseMetadata.Headers.ContainsName("Content-Length"))
                        {
                            _serverFlowState = ServerFlowState.RedirectingFixedSizeContent;
                            contentBytesCount = transformedResponseMetadata.Headers.GetContentLength();
                        }
                        else if (transformedResponseMetadata.Headers.ContainsName("Transfer-Encoding"))
                        {
                            var transferEncoding = transformedResponseMetadata.Headers.GetTransferEncoding();
                            if (transferEncoding == HttpTransferEncoding.Chunked)
                            {
                                _serverFlowState = ServerFlowState.RedirectingChunkedContent;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                    }
                }
                else if (_serverFlowState == ServerFlowState.RedirectingFixedSizeContent)
                {
                    _serverFixedSizeContentRedirector.Redirect(contentBytesCount);
                    contentBytesCount = 0;
                    _serverFlowState = ServerFlowState.Idle;

                }
                else if (_serverFlowState == ServerFlowState.RedirectingChunkedContent)
                {
                    _serverChunkedContentRedirector.Redirect();
                    _serverFlowState = ServerFlowState.Idle;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private bool IsSecure => _conf.CertificateId != null;

        private HttpRequestMetadata TransformRequestMetadata(HttpRequestMetadata requestMetadata)
        {
            var requestMetadataBuilder = new HttpRequestMetadataBuilder(requestMetadata);
            requestMetadataBuilder.Headers.Replace("Host", "localhost:53808"); // todo00000000[ak]
            var transformedRequestMetadata = requestMetadataBuilder.Build();

            return transformedRequestMetadata;
        }

        private HttpResponseMetadata TransformResponseMetadata(HttpResponseMetadata responseMetadata)
        {
            if (responseMetadata.Line.Code == HttpStatusCode.Found)
            {
                var location = responseMetadata.Headers.GetLocation();

                var locationIsAbsolute = location.StartsWith("http://") || location.StartsWith("https://");
                if (locationIsAbsolute)
                {
                    var uri = new Uri(location);

                    if (
                        uri.Authority == _domesticAuthority ||
                        uri.Authority == _domesticAuthorityWithPort)
                    {
                        var changedLocation = this.BuildExternalUrl(uri.PathAndQuery);

                        var responseMetadataBuilder = new HttpResponseMetadataBuilder(responseMetadata);
                        responseMetadataBuilder.Headers.Replace("Location", changedLocation);

                        responseMetadata = responseMetadataBuilder.Build();
                    }
                }
            }

            return responseMetadata; // todo0[ak] temp
        }

        private string BuildExternalUrl(string pathAndQuery)
        {
            return $"{_protocol}://{_conf.ExternalHostName}{pathAndQuery}";
        }
    }
}

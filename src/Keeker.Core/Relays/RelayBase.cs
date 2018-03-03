using Keeker.Core.Conf;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Keeker.Core.Relays
{
    public class RelayBase
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

        private readonly RelayPlainConf _conf;

        private readonly KeekStream _clientStream;
        private ClientFlowState _clientFlowState;
        private readonly AutoBuffer _clientBuffer;
        private readonly FixedSizeContentRedirector _clientRedirector;

        private readonly KeekStream _serverStream;
        private ServerFlowState _serverFlowState;
        private readonly AutoBuffer _serverBuffer;
        private readonly FixedSizeContentRedirector _serverRedirector;

        public RelayBase(Stream innerClientStream, RelayPlainConf conf)
        {
            _conf = (conf ?? throw new ArgumentNullException(nameof(conf))).Clone();

            _clientStream = new KeekStream(innerClientStream);
            _clientBuffer = new AutoBuffer();
            _clientRedirector = new FixedSizeContentRedirector(_clientStream, _serverStream, _clientBuffer);

            var tcpclient = new TcpClient();
            tcpclient.Connect(_conf.EndPoint);
            var serverNetworkStream = tcpclient.GetStream();

            _serverStream = new KeekStream(serverNetworkStream);
            _serverBuffer = new AutoBuffer();
            _serverRedirector = new FixedSizeContentRedirector(_serverStream, _clientStream, _serverBuffer);
        }

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
                        var transformedRequestMetadata = this.TransformRequestMetadata(requestMetadata);
                        var transformedRequestMetadataBytes = transformedRequestMetadata.ToArray();
                        _serverStream.Write(transformedRequestMetadataBytes, 0, transformedRequestMetadataBytes.Length);

                        if (transformedRequestMetadata.Headers.ContainsName("Content-Length"))
                        {
                            _clientFlowState = ClientFlowState.RedirectingFixedSizeContent;
                            contentBytesCount = transformedRequestMetadata.Headers.GetContentLength();
                        }
                    }
                }
                else if (_clientFlowState == ClientFlowState.RedirectingFixedSizeContent)
                {
                    //this.RedirectClientContent(contentBytesCount);
                    _clientRedirector.Redirect(contentBytesCount);
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
                    //this.RedirectServerContent(contentBytesCount);
                    _serverRedirector.Redirect(contentBytesCount);
                    contentBytesCount = 0;
                    _serverFlowState = ServerFlowState.Idle;

                }
                else if (_serverFlowState == ServerFlowState.RedirectingChunkedContent)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            //var from = _serverStream;
            //var to = _clientStream;

            //var buffer = new byte[65536];

            //while (true)
            //{
            //    try
            //    {
            //        var bytesCount = from.Read(buffer, 0, buffer.Length);

            //        if (bytesCount == 0)
            //        {
            //            throw new NotImplementedException();
            //        }

            //        to.Write(buffer, 0, bytesCount);
            //    }
            //    catch
            //    {
            //        try
            //        {
            //            from.Close();
            //        }
            //        catch
            //        {
            //            // don't care; dismiss
            //        }

            //        try
            //        {
            //            to.Close();
            //        }
            //        catch
            //        {
            //            // don't care; dismiss
            //        }

            //        // don't care, just exit.
            //        break;
            //    }
            //}
        }

        private HttpRequestMetadata TransformRequestMetadata(HttpRequestMetadata requestMetadata)
        {
            var requestMetadataBuilder = new HttpRequestMetadataBuilder(requestMetadata);
            requestMetadataBuilder.Headers.Replace("Host", "localhost:53808"); // todo00000000[ak]
            var transformedRequestMetadata = requestMetadataBuilder.Build();

            return transformedRequestMetadata;
        }

        //private void RedirectClientContent(int contentBytesCount)
        //{
        //    const int CONTENT_TRANSFER_PORTION_SIZE = 10000;

        //    var remaining = contentBytesCount;
        //    _clientBuffer.Allocate(CONTENT_TRANSFER_PORTION_SIZE);
        //    var buffer = _clientBuffer.Buffer; // lazy for typing :)

        //    while (true)
        //    {
        //        if (remaining == 0)
        //        {
        //            break;
        //        }

        //        var portionSize = Math.Min(remaining, buffer.Length);
        //        var bytesReadCount = _clientStream.Read(buffer, 0, portionSize);
        //        _serverStream.Write(buffer, 0, bytesReadCount);

        //        remaining -= bytesReadCount;
        //    }
        //}

        private HttpResponseMetadata TransformResponseMetadata(HttpResponseMetadata responseMetadata)
        {
            return responseMetadata; // todo0[ak] temp
        }

        //private void RedirectServerContent(int contentBytesCount)
        //{
        //    const int CONTENT_TRANSFER_PORTION_SIZE = 10000;

        //    var remaining = contentBytesCount;
        //    _serverBuffer.Allocate(CONTENT_TRANSFER_PORTION_SIZE);
        //    var buffer = _serverBuffer.Buffer;

        //    while (true)
        //    {
        //        if (remaining == 0)
        //        {
        //            break;
        //        }

        //        var portionSize = Math.Min(remaining, buffer.Length);
        //        var bytesReadCount = _serverStream.Read(buffer, 0, portionSize);
        //        _clientStream.Write(buffer, 0, bytesReadCount);

        //        remaining -= bytesReadCount;
        //    }
        //}
    }
}

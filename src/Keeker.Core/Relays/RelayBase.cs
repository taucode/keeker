using Keeker.Core.Conf;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Keeker.Core.Relays
{
    public class RelayBase
    {
        private const int REDIRECT_CONTENT_BUFFER_SIZE = 1000;

        private enum ClientFlowState
        {
            Unknown = 0,
            ReadingHeader,
            ReadingContent,
        }

        private enum ServerFlowState
        {
            Unknown = 0,
            ReadingHeader,
            ReadingContent,
        }

        private readonly RelayPlainConf _conf;

        private readonly KeekStream _clientStream;
        private ClientFlowState _clientFlowState;
        private readonly AutoBuffer _clientBuffer;

        private readonly KeekStream _serverStream;
        private ServerFlowState _serverFlowState;
        private readonly AutoBuffer _serverBuffer;

        public RelayBase(Stream innerClientStream, RelayPlainConf conf)
        {
            _conf = (conf ?? throw new ArgumentNullException(nameof(conf))).Clone();

            _clientStream = new KeekStream(innerClientStream);
            _clientBuffer = new AutoBuffer();

            var tcpclient = new TcpClient();
            tcpclient.Connect(_conf.EndPoint);
            var serverNetworkStream = tcpclient.GetStream();
            _serverStream = new KeekStream(serverNetworkStream);
            _serverBuffer = new AutoBuffer();

            //_redirectContentBuffer = new byte[REDIRECT_CONTENT_BUFFER_SIZE];
        }

        public void Start()
        {
            new Task(this.ClientRoutine).Start();
            new Task(this.ServerRoutine).Start();
        }

        private void ClientRoutine()
        {
            const int PORTION_SIZE = 1000;

            _clientFlowState = ClientFlowState.ReadingHeader;;
            var contentBytesCount = 0;

            while (true)
            {
                if (_clientFlowState == ClientFlowState.ReadingHeader)
                {
                    var bytesReadCount = _clientStream.ReadInnerStream(PORTION_SIZE);

                    if (bytesReadCount == 0)
                    {
                        throw new NotImplementedException(); // socket has been shut down
                    }

                    var index = _clientStream.PeekIndexOf(HttpHelper.CrLfCrLfBytes);
                    if (index >= 0)
                    {
                        var metadataLength = index + HttpHelper.CrLfCrLfBytes.Length;
                        //var buffer = new byte[metadataLength];

                        _clientBuffer.Allocate(metadataLength);
                        _clientStream.Read(_clientBuffer.Buffer, 0, metadataLength);

                        var requestMetadata = HttpRequestMetadata.Parse(_clientBuffer.Buffer, 0);
                        var transformedRequestMetadata = this.TransformRequestMetadata(requestMetadata);
                        var transformedRequestMetadataBytes = transformedRequestMetadata.ToArray();
                        _serverStream.Write(transformedRequestMetadataBytes, 0, transformedRequestMetadataBytes.Length);

                        if (transformedRequestMetadata.Headers.ContainsName("Content-Length"))
                        {
                            _clientFlowState = ClientFlowState.ReadingContent;
                            contentBytesCount = transformedRequestMetadata.Headers.GetContentLength();
                        }
                    }
                }
                else if (_clientFlowState == ClientFlowState.ReadingContent)
                {
                    this.RedirectClientContent(contentBytesCount);
                    contentBytesCount = 0;
                    _clientFlowState = ClientFlowState.ReadingHeader;
                }
                else
                {
                    throw new ApplicationException(); // todo2[ak] wtf
                }
            }
        }

        private void ServerRoutine()
        {
            var from = _serverStream;
            var to = _clientStream;

            var buffer = new byte[65536];

            while (true)
            {
                try
                {
                    var bytesCount = from.Read(buffer, 0, buffer.Length);

                    if (bytesCount == 0)
                    {
                        throw new NotImplementedException();
                    }

                    to.Write(buffer, 0, bytesCount);
                }
                catch
                {
                    try
                    {
                        from.Close();
                    }
                    catch
                    {
                        // don't care; dismiss
                    }

                    try
                    {
                        to.Close();
                    }
                    catch
                    {
                        // don't care; dismiss
                    }

                    // don't care, just exit.
                    break;
                }
            }
        }

        private HttpRequestMetadata TransformRequestMetadata(HttpRequestMetadata requestMetadata)
        {
            var requestMetadataBuilder = new HttpRequestMetadataBuilder(requestMetadata);
            requestMetadataBuilder.Headers.Replace("Host", "localhost:53808"); // todo00000000[ak]
            var transformedRequestMetadata = requestMetadataBuilder.Build();

            return transformedRequestMetadata;
        }

        private void RedirectClientContent(int contentBytesCount)
        {
            var remaining = contentBytesCount;
            var buffer = _clientBuffer.Buffer; // lazy for typing :)

            while (true)
            {
                if (remaining == 0)
                {
                    break;
                }

                var portionSize = Math.Min(remaining, buffer.Length);
                var bytesReadCount = _clientStream.Read(buffer, 0, portionSize);
                _serverStream.Write(buffer, 0, bytesReadCount);

                remaining -= bytesReadCount;
            }
        }
    }
}

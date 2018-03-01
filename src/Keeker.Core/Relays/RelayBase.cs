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

        private readonly KeekStream _clientStream;
        private readonly KeekStream _serverStream;
        private readonly RelayPlainConf _conf;

        private ClientFlowState _clientFlowState;

        private readonly byte[] _redirectContentBuffer;

        public RelayBase(Stream innerClientStream, RelayPlainConf conf)
        {
            _conf = (conf ?? throw new ArgumentNullException(nameof(conf))).Clone();

            _clientStream = new KeekStream(innerClientStream);

            var tcpclient = new TcpClient();
            tcpclient.Connect(_conf.GetEndPoint());
            var serverNetworkStream = tcpclient.GetStream();
            _serverStream = new KeekStream(serverNetworkStream);

            _redirectContentBuffer = new byte[REDIRECT_CONTENT_BUFFER_SIZE];
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
                    _clientStream.ReadInnerStream(PORTION_SIZE);
                    var index = _clientStream.PeekIndexOf(HttpHelper.CrLfCrLfBytes);
                    if (index >= 0)
                    {
                        var metadataLength = index + HttpHelper.CrLfCrLfBytes.Length;
                        var buffer = new byte[metadataLength];
                        _clientStream.Read(buffer, 0, metadataLength);

                        var requestMetadata = HttpRequestMetadata.Parse(buffer, 0);
                        var transformedRequestMetadata = this.TransformRequestMetadata(requestMetadata);
                        var transformedRequestMetadataBytes = transformedRequestMetadata.ToArray();
                        _serverStream.Write(transformedRequestMetadataBytes, 0, transformedRequestMetadataBytes.Length);

                        if (transformedRequestMetadata.Headers.ContainsName("Content-Length"))
                        {
                            _clientFlowState = ClientFlowState.ReadingContent;
                            contentBytesCount = transformedRequestMetadata.Headers.GetContentLength();
                        }
                    }
                    else
                    {
                        // wat? todo0[ak]
                    }
                }
                else if (_clientFlowState == ClientFlowState.ReadingContent)
                {
                    this.RedirectContent(contentBytesCount);
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

        private void RedirectContent(int contentBytesCount)
        {
            var remaining = contentBytesCount;
            var buffer = _redirectContentBuffer; // lazy for typing :)

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

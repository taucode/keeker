using System;
using System.IO;
using System.Threading.Tasks;

namespace Keeker.Core
{
    public class WebRequestRelay : IDisposable
    {
        private const int REDIRECT_CONTENT_BUFFER_SIZE = 1000;

        private enum State
        {
            Unknown = 0,
            ReadingHeader,
            ReadingContent,
        }

        private readonly KeekStream _clientKeekStream;
        private readonly Stream _serverStream;
        private State _state;
        private readonly byte[] _redirectContentBuffer;

        public WebRequestRelay(KeekStream clientKeekStream, Stream serverStream)
        {
            _clientKeekStream = clientKeekStream ?? throw new ArgumentNullException(nameof(clientKeekStream));
            _serverStream = serverStream ?? throw new ArgumentNullException(nameof(serverStream));

            _redirectContentBuffer = new byte[REDIRECT_CONTENT_BUFFER_SIZE];
        }

        public void Start()
        {
            new Task(this.ReadingRoutine).Start();
        }

        private void ReadingRoutine()
        {
            const int PORTION_SIZE = 1000;

            _state = State.ReadingHeader;
            var contentBytesCount = 0;

            while (true)
            {
                if (_state == State.ReadingHeader)
                {
                    _clientKeekStream.ReadInnerStream(PORTION_SIZE);
                    var index = _clientKeekStream.PeekIndexOf(HttpHelper.CrLfCrLfBytes);
                    if (index >= 0)
                    {
                        var metadataLength = index + HttpHelper.CrLfCrLfBytes.Length;
                        var buffer = new byte[metadataLength];
                        _clientKeekStream.Read(buffer, 0, metadataLength);

                        var requestMetadata = HttpRequestMetadata.Parse(buffer, 0);
                        var transformedRequestMetadata = this.TransformRequestMetadata(requestMetadata);
                        var transformedRequestMetadataBytes = transformedRequestMetadata.ToArray();
                        _serverStream.Write(transformedRequestMetadataBytes, 0, transformedRequestMetadataBytes.Length);

                        if (transformedRequestMetadata.Headers.ContainsName("Content-Length"))
                        {
                            _state = State.ReadingContent;
                            contentBytesCount = transformedRequestMetadata.Headers.GetContentLength();
                        }
                    }
                    else
                    {
                        // wat? todo0[ak]
                    }
                }
                else if (_state == State.ReadingContent)
                {
                    this.RedirectContent(contentBytesCount);
                    contentBytesCount = 0;
                    _state = State.ReadingHeader;
                }
            }
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
                var bytesReadCount = _clientKeekStream.Read(buffer, 0, portionSize);
                _serverStream.Write(buffer, 0, bytesReadCount);

                remaining -= bytesReadCount;
            }
        }

        private HttpRequestMetadata TransformRequestMetadata(HttpRequestMetadata requestMetadata)
        {
            var requestMetadataBuilder = new HttpRequestMetadataBuilder(requestMetadata);
            requestMetadataBuilder.Headers.Replace("Host", "localhost:53808"); // todo00000000[ak]
            var transformedRequestMetadata = requestMetadataBuilder.Build();

            return transformedRequestMetadata;
        }

        public void Dispose()
        {
            _clientKeekStream.Dispose();
        }
    }
}

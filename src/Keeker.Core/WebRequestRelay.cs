using System;
using System.IO;
using System.Threading.Tasks;

namespace Keeker.Core
{
    public class WebRequestRelay : IDisposable
    {
        private static readonly byte[] CRLF_CRLF_BYTES = "\r\n\r\n".ToAsciiBytes();

        private readonly KeekStream _clientKeekStream;
        private readonly Stream _serverStream;
        

        public WebRequestRelay(KeekStream clientKeekStream, Stream serverStream)
        {
            _clientKeekStream = clientKeekStream ?? throw new ArgumentNullException(nameof(clientKeekStream));
            _serverStream = serverStream ?? throw new ArgumentNullException(nameof(serverStream));
        }

        public void Start()
        {
            new Task(this.ReadingRoutine).Start();
        }

        private void ReadingRoutine()
        {
            const int PORTION_SIZE = 1000;

            while (true)
            {
                _clientKeekStream.ReadInnerStream(PORTION_SIZE);
                var index = _clientKeekStream.PeekIndexOf(CRLF_CRLF_BYTES);
                if (index >= 0)
                {
                    var metadataLength = index + CRLF_CRLF_BYTES.Length;
                    var buffer = new byte[metadataLength];
                    _clientKeekStream.Read(buffer, 0, metadataLength);

                    var requestMetadata = HttpRequestMetadata.Parse(buffer, 0);
                    var transformedRequestMetadata = this.TransformRequestMetadata(requestMetadata);
                    var transformedRequestMetadataBytes = transformedRequestMetadata.ToArray();
                    _serverStream.Write(transformedRequestMetadataBytes, 0, transformedRequestMetadataBytes.Length);
                }
                else
                {
                    // wat? todo0[ak]
                }
            }
        }

        private HttpRequestMetadata TransformRequestMetadata(HttpRequestMetadata requestMetadata)
        {
            return requestMetadata; // todo000[temp]
        }

        public void Dispose()
        {
            _clientKeekStream.Dispose();
        }
    }
}

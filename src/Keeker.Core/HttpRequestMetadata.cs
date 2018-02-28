using System.IO;

namespace Keeker.Core
{
    public class HttpRequestMetadata
    {
        private static readonly byte[] CRLF_BYTES = "\r\n".ToAsciiBytes();

        public HttpRequestMetadata(HttpRequestLine line, HttpHeaderCollection headers)
        {
            this.Line = line;
            this.Headers = headers;
        }

        public HttpRequestLine Line { get; }

        public HttpHeaderCollection Headers { get; }

        public byte[] ToArray()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteAll(this.Line.ToArray());
                stream.WriteAll(this.Headers.ToArray());

                stream.WriteAll(CRLF_BYTES);

                return stream.ToArray();
            }
        }

        public static HttpRequestMetadata Parse(byte[] buffer, int offset)
        {
            // parse line
            var line = HttpRequestLine.Parse(buffer, 0);
            offset += line.ByteCount;

            // parse headers
            var headers = HttpHeaderCollection.Parse(buffer, offset);

            var metadata = new HttpRequestMetadata(line, headers);

            return metadata;
        }
    }
}
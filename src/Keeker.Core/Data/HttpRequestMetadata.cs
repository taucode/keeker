using System.IO;

namespace Keeker.Core.Data
{
    public class HttpRequestMetadata : IHttpMetadata
    {
        public HttpRequestMetadata(HttpRequestLine line, HttpHeaderCollection headers)
        {
            this.Line = line;
            this.Headers = headers;
        }

        public HttpRequestLine Line { get; }

        public HttpHeaderCollection Headers { get; }

        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteAll(this.Line.ToArray());
                stream.WriteAll(this.Headers.ToArray());

                stream.WriteAll(CoreHelper.CrLfBytes);

                return stream.ToArray();
            }
        }

        public static HttpRequestMetadata Parse(byte[] buffer, int start)
        {
            // parse line
            var line = HttpRequestLine.Parse(buffer, 0);
            start += line.ByteCount;

            // parse headers
            var headers = HttpHeaderCollection.Parse(buffer, start);

            var metadata = new HttpRequestMetadata(line, headers);

            return metadata;
        }
    }
}
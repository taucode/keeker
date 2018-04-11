using System.IO;

namespace Keeker.Convey.Data
{
    public class HttpRequestMetadata : IHttpMetadata
    {
        public HttpRequestMetadata(Convey.Data.HttpRequestLine line, Convey.Data.HttpHeaderCollection headers)
        {
            this.Line = line;
            this.Headers = headers;
        }

        public Convey.Data.HttpRequestLine Line { get; }

        public Convey.Data.HttpHeaderCollection Headers { get; }

        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteAll(this.Line.ToArray());
                stream.WriteAll(this.Headers.ToArray());

                stream.WriteAll(Helper.CrLfBytes);

                return stream.ToArray();
            }
        }

        public static HttpRequestMetadata Parse(byte[] buffer, int start)
        {
            // parse line
            var line = Convey.Data.HttpRequestLine.Parse(buffer, 0);
            start += line.ByteCount;

            // parse headers
            var headers = Convey.Data.HttpHeaderCollection.Parse(buffer, start);

            var metadata = new HttpRequestMetadata(line, headers);

            return metadata;
        }
    }
}
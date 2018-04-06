using System.IO;

namespace Keeker.Convey.Data
{
    public class HttpResponseMetadata : IHttpMetadata
    {
        public HttpResponseMetadata(HttpStatusLine line, Convey.Data.HttpHeaderCollection headers)
        {
            this.Line = line;
            this.Headers = headers;
        }

        public HttpStatusLine Line { get; }

        public Convey.Data.HttpHeaderCollection Headers { get; }

        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteAll(this.Line.ToArray());
                stream.WriteAll(this.Headers.ToArray());

                stream.WriteAll(TauHelper.CrLfBytes);

                return stream.ToArray();
            }
        }

        public static HttpResponseMetadata Parse(byte[] buffer, int start)
        {
            // parse status line
            var line = HttpStatusLine.Parse(buffer, 0);
            start += line.ByteCount;

            // parse headers
            var headers = Convey.Data.HttpHeaderCollection.Parse(buffer, start);

            var metadata = new HttpResponseMetadata(line, headers);

            return metadata;
        }
    }
}
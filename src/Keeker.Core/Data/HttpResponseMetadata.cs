using System;
using System.IO;
using System.Text;

namespace Keeker.Core.Data
{
    public class HttpResponseMetadata : IHttpMetadata
    {
        public HttpResponseMetadata(HttpStatusLine line, HttpHeaderCollection headers)
        {
            this.Line = line ?? throw new ArgumentNullException(nameof(line));
            this.Headers = headers ?? throw new ArgumentNullException(nameof(headers));
        }

        public HttpStatusLine Line { get; }

        public HttpHeaderCollection Headers { get; }

        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteAll(this.Line.ToArray());
                stream.WriteAll(this.Headers.ToArray());

                return stream.ToArray();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(this.Line);
            sb.Append(this.Headers);

            var res = sb.ToString();
            return res;
        }

        public static HttpResponseMetadata Parse(byte[] buffer, int start)
        {
            // parse status line
            var line = HttpStatusLine.Parse(buffer, start);
            start += line.ByteCount;

            // parse headers
            var headers = HttpHeaderCollection.Parse(buffer, start);

            var metadata = new HttpResponseMetadata(line, headers);

            return metadata;
        }
    }
}
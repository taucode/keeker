using System;
using System.IO;
using System.Text;

namespace Keeker.Core.Data
{
    public class HttpRequestMetadata : HttpMetadataBase
    {
        public HttpRequestMetadata(HttpRequestLine line, HttpHeaderCollection headers)
        {
            this.Line = line ?? throw new ArgumentNullException(nameof(line));
            this.Headers = headers ?? throw new ArgumentNullException(nameof(headers));
        }

        public HttpRequestLine Line { get; }

        public HttpHeaderCollection Headers { get; }

        public override byte[] Serialize()
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

        public static HttpRequestMetadata Parse(byte[] buffer, int start)
        {
            // parse line
            var line = HttpRequestLine.Parse(buffer, start);
            start += line.ByteCount;

            // parse headers
            var headers = HttpHeaderCollection.Parse(buffer, start);

            var metadata = new HttpRequestMetadata(line, headers);

            return metadata;
        }
    }
}
using System.Collections.Generic;
using System.IO;

namespace Keeker.Core
{
    public class HttpHeaderCollection
    {
        private static readonly byte[] CRLF_CRLF_BYTES = "\r\n\r\n".ToAsciiBytes();
        private static readonly byte[] CRLF_BYTES = "\r\n".ToAsciiBytes();

        private readonly List<HttpHeader> _headers;

        public HttpHeaderCollection()
        {
            _headers = new List<HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            _headers.Add(header);
        }

        public byte[] ToArray()
        {
            using (var stream = new MemoryStream())
            {
                foreach (var header in _headers)
                {
                    stream.WriteAll(header.ToArray());
                }

                return stream.ToArray();
            }
        }

        public static HttpHeaderCollection Parse(byte[] buffer, int offset)
        {
            var headers = new HttpHeaderCollection();

            while (true)
            {
                var header = HttpHeader.Parse(buffer, offset);
                if (header == null)
                {
                    break;
                }

                headers.Add(header);
                offset += header.ByteCount;
            }

            return headers;
        }
    }
}
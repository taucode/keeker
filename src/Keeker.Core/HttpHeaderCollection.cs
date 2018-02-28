using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Keeker.Core
{
    public class HttpHeaderCollection : IEnumerable<HttpHeader>
    {
        private readonly List<HttpHeader> _headers;

        public HttpHeaderCollection()
        {
            _headers = new List<HttpHeader>();
        }

        public HttpHeaderCollection(IEnumerable<HttpHeader> headers)
        {
            _headers = new List<HttpHeader>(headers);
        }

        public void Add(HttpHeader header)
        {
            _headers.Add(header);
        }

        public bool ContainsName(string name)
        {
            var contains = _headers.Any(x => x.Name == name);
            return contains;
        }

        public int GetContentLength()
        {
            return _headers.Single(x => x.Name == "Content-Length").Value.ToInt32();
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

        public IEnumerator<HttpHeader> GetEnumerator()
        {
            return _headers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Keeker.Core.Data
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

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<HttpHeader> GetEnumerator()
        {
            return _headers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static HttpHeaderCollection Parse(byte[] buffer, int start)
        {
            var headers = new HttpHeaderCollection();

            while (true)
            {
                var header = HttpHeader.Parse(buffer, start);
                if (header == null)
                {
                    break;
                }

                headers.Add(header);
                start += header.ByteCount;
            }

            return headers;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Keeker.Convey.Data
{
    public class HttpHeaderCollection : IEnumerable<Convey.Data.HttpHeader>
    {
        private readonly List<Convey.Data.HttpHeader> _headers;

        public HttpHeaderCollection()
        {
            _headers = new List<Convey.Data.HttpHeader>();
        }

        public HttpHeaderCollection(IEnumerable<Convey.Data.HttpHeader> headers)
        {
            _headers = new List<Convey.Data.HttpHeader>(headers);
        }

        public void Add(Convey.Data.HttpHeader header)
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

        public IEnumerator<Convey.Data.HttpHeader> GetEnumerator()
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
                var header = Convey.Data.HttpHeader.Parse(buffer, start);
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
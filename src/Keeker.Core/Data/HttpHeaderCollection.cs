using Keeker.Core.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            if (headers.Any(x => x == null))
            {
                throw new ArgumentException("'headers' cannot contain nulls", nameof(headers));
            }

            _headers = new List<HttpHeader>(headers);
        }

        public void Add(HttpHeader header)
        {
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            _headers.Add(header);
        }

        public bool ContainsName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

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

                stream.WriteAll(CoreHelper.CrLfBytes);

                return stream.ToArray();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var header in _headers)
            {
                sb.Append(header);
            }

            sb.Append(CoreHelper.CrLf);
            return sb.ToString();
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
                try
                {
                    var header = HttpHeader.Parse(buffer, start);
                    if (header == null)
                    {
                        break;
                    }

                    headers.Add(header);
                    start += header.ByteCount;
                }
                catch (Exception ex)
                {
                    throw new BadHttpDataException("Could not parse header", ex);
                }
            }

            return headers;
        }
    }
}
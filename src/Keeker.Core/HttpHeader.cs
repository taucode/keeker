using System;
using System.Text;

namespace Keeker.Core
{
    public class HttpHeader
    {
        private const string CRLF = "\r\n";
        private static readonly byte[] CRLF_BYTES = CRLF.ToAsciiBytes();

        public HttpHeader(string name, string value)
        {
            this.Name = name;
            this.Value = value;

            this.ByteCount =
                this.Name.Length + 1 + 1 + // ':' and ' '
                this.Value.Length + CRLF_BYTES.Length;
        }

        public string Name { get; }

        public string Value { get; }

        public int ByteCount { get; }

        public override string ToString()
        {
            return $"{this.Name}: {this.Value}{CRLF}";
        }

        public byte[] ToArray()
        {
            return Encoding.ASCII.GetBytes(this.ToString());
        }

        public static HttpHeader Parse(byte[] buffer, int start)
        {
            var crlfIndex = buffer.IndexOfSubarray(CRLF_BYTES, start);
            if (crlfIndex == start)
            {
                return null;
            }
            var count = crlfIndex - start;
            var line = buffer.GetAsciiSubstring(start, count);

            var colonPos = line.IndexOf(':');
            if (colonPos == -1)
            {
                throw new ApplicationException(); // todo1[ak]
            }

            var name = line.Substring(0, colonPos);
            var value = line.Substring(colonPos + 2); // skip ':' and following ' '.

            return new HttpHeader(name, value);
        }
    }
}
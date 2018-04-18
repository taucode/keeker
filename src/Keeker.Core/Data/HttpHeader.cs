using System;
using Keeker.Core.Exceptions;
using System.Text;

namespace Keeker.Core.Data
{
    public class HttpHeader
    {
        public HttpHeader(string name, string value)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Value = value ?? throw new ArgumentNullException(nameof(value));

            this.ByteCount =
                this.Name.Length + 1 + 1 + // ':' and ' '
                this.Value.Length + CoreHelper.CrLfBytes.Length;
        }

        public string Name { get; }

        public string Value { get; }

        public int ByteCount { get; }

        public override string ToString()
        {
            return $"{this.Name}: {this.Value}{CoreHelper.CrLf}";
        }

        public byte[] ToArray()
        {
            return Encoding.ASCII.GetBytes(this.ToString());
        }

        public static HttpHeader Parse(byte[] buffer, int start)
        {
            var crlfIndex = buffer.IndexOfSubarray(CoreHelper.CrLfBytes, start);
            if (crlfIndex == start)
            {
                return null;
            }
            var count = crlfIndex - start;
            var line = buffer.ToAsciiString(start, count);

            var colonPos = line.IndexOf(':');
            if (colonPos == -1)
            {
                throw new BadHttpDataException();
            }

            var name = line.Substring(0, colonPos);
            var value = line.Substring(colonPos + 2); // skip ':' and following ' '.

            return new HttpHeader(name, value);
        }
    }
}
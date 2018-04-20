using Keeker.Core.Exceptions;
using System;
using System.Text;

namespace Keeker.Core.Data
{
    public class HttpHeader
    {
        public HttpHeader(string name, string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!CoreHelper.IsValidHeaderName(name))
            {
                throw new ArgumentException($"Bad header name: '{name}'", nameof(name));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!CoreHelper.IsValidHeaderValue(value))
            {
                throw new ArgumentException($"Bad header value: '{value}'", nameof(value));
            }


            this.Name = name;
            this.Value = value;

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
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (start < 0 || start >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            var crlfIndex = buffer.IndexOfSubarray(CoreHelper.CrLfBytes, start);

            if (crlfIndex == -1)
            {
                throw new BadHttpDataException("Header-delimiting CRLF not found");
            }

            if (crlfIndex == start)
            {
                return null;
            }

            var count = crlfIndex - start;
            var line = buffer.ToAsciiString(start, count);

            var colonPos = line.IndexOf(':');
            if (colonPos == -1)
            {
                throw new BadHttpDataException("Header-splitting colon not found");
            }

            var spacePos = colonPos + 1;
            if (spacePos >= line.Length || line[spacePos] != ' ')
            {
                throw new BadHttpDataException("Could not parse HTTP header");
            }

            var name = line.Substring(0, colonPos);
            var value = line.Substring(colonPos + 2); // skip ':' and following ' '.

            try
            {
                return new HttpHeader(name, value);
            }
            catch (ArgumentException ex)
            {
                throw new BadHttpDataException("Could not parse HTTP header", ex);
            }
        }
    }
}
using System;
using System.Net.Http;
using System.Text;

namespace Keeker.Core
{
    public class HttpRequestLine
    {
        private const byte SPACE_BYTE = (byte)' ';

        public HttpRequestLine(HttpMethod method, string requestUri, string version)
        {
            this.Method = method;
            this.RequestUri = requestUri;
            this.Version = version;

            this.ByteCount =
                this.Method.ToString().Length + 1 +
                this.RequestUri.Length + 1 +
                this.Version.Length + HttpHelper.CrLfBytes.Length;
        }

        public HttpMethod Method { get; }
        public string RequestUri { get; }
        public string Version { get; }

        public override string ToString()
        {
            return $"{this.Method} {this.RequestUri} {this.Version}{HttpHelper.CrLf}";
        }

        public byte[] ToArray()
        {
            return Encoding.ASCII.GetBytes(this.ToString());
        }

        public int ByteCount { get; }

        public static HttpRequestLine Parse(byte[] buffer, int offset)
        {
            var crLfIndex = buffer.IndexOfSubarray(HttpHelper.CrLfBytes, offset, -1);
            if (crLfIndex == -1)
            {
                throw new ApplicationException(); // todo1[ak]
            }

            // method
            var indexOfSpaceAfterMethod = buffer.IndexOf(SPACE_BYTE, offset);
            if (indexOfSpaceAfterMethod == -1)
            {
                throw new ApplicationException(); // todo1[ak]
            }

            var length = indexOfSpaceAfterMethod - offset;
            var method = buffer.GetAsciiSubstring(offset, length);

            // advance
            offset = indexOfSpaceAfterMethod + 1; // skip ' '

            // uri
            var indexOfSpaceAfterUri = buffer.IndexOf(SPACE_BYTE, offset);

            length = indexOfSpaceAfterUri - offset;
            var uri = buffer.GetAsciiSubstring(offset, length);

            // advance
            offset = indexOfSpaceAfterUri + 1; // skip ' '

            // http version
            length = crLfIndex - offset;
            var version = buffer.GetAsciiSubstring(offset, length);

            var line = new HttpRequestLine(new HttpMethod(method), uri, version);
            return line;
        }
    }
}

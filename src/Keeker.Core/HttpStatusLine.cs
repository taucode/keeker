using System;
using System.Net;
using System.Text;

namespace Keeker.Core
{
    public class HttpStatusLine
    {
        private const byte SPACE_BYTE = (byte)' ';

        public HttpStatusLine(string version, HttpStatusCode code, string reason)
        {
            this.Version = version;
            this.Code = code;
            this.Reason = reason;

            this.ByteCount =
                this.Version.Length + 1 +
                ((int)this.Code).ToString().Length + 1 +
                this.Reason.Length + HttpHelper.CrLfBytes.Length;
        }

        public string Version { get; }

        public HttpStatusCode Code { get; }

        public string Reason { get; }

        public override string ToString()
        {
            return $"{this.Version} {(int)this.Code} {this.Reason}";
        }

        public byte[] ToArray()
        {
            return Encoding.ASCII.GetBytes(this.ToString());
        }

        public int ByteCount { get; }

        public static HttpStatusLine Parse(byte[] buffer, int start)
        {
            var crLfIndex = buffer.IndexOfSubarray(HttpHelper.CrLfBytes, start, -1);
            if (crLfIndex == -1)
            {
                throw new ApplicationException(); // todo1[ak]
            }

            // version
            var indexOfSpaceAfterVersion = buffer.IndexOf(SPACE_BYTE, start);
            if (indexOfSpaceAfterVersion == -1)
            {
                throw new ApplicationException(); // todo1[ak]
            }

            var length = indexOfSpaceAfterVersion - start;
            var version = buffer.ToAsciiString(start, length);

            // advance
            start = indexOfSpaceAfterVersion + 1; // skip ' '

            // code
            var indexOfSpaceAfterCode = buffer.IndexOf(SPACE_BYTE, start);

            length = indexOfSpaceAfterCode - start;
            var codeNumber = buffer.ToAsciiString(start, length).ToInt32();
            var code = (HttpStatusCode)codeNumber;

            // advance
            start = indexOfSpaceAfterCode + 1; // skip ' '

            // http version
            length = crLfIndex - start;
            var reason = buffer.ToAsciiString(start, length);

            var line = new HttpStatusLine(version, code, reason);
            return line;
        }
    }
}
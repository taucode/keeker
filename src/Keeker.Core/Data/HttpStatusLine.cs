using Keeker.Core.Exceptions;
using System;
using System.Net;
using System.Text;

namespace Keeker.Core.Data
{
    public class HttpStatusLine
    {
        private const byte SPACE_BYTE = (byte)' ';

        public HttpStatusLine(string version, HttpStatusCode code, string reason)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (!CoreHelper.IsValidHttpVersion(version))
            {
                throw new ArgumentException("Invalid HTTP version", nameof(version)); // todo1[ak] copy/paste
            }

            if (reason == null)
            {
                throw new ArgumentNullException(nameof(reason));
            }

            if (!CoreHelper.IsValidStatusReason(reason))
            {
                throw new ArgumentException("Invalid status reason", nameof(reason));
            }

            this.Version = version;
            this.Code = code;
            this.Reason = reason;

            this.ByteCount =
                this.Version.Length + 1 +
                ((int)this.Code).ToString().Length + 1 +
                this.Reason.Length + CoreHelper.CrLfBytes.Length;
        }

        public HttpStatusLine(HttpStatusCode code)
            : this(CoreHelper.HttpVersion11, code, code.ToReason())
        {
        }

        public string Version { get; }

        public HttpStatusCode Code { get; }

        public string Reason { get; }

        public override string ToString()
        {
            return $"{this.Version} {(int)this.Code} {this.Reason}{CoreHelper.CrLf}";
        }

        public byte[] ToArray()
        {
            return Encoding.ASCII.GetBytes(this.ToString());
        }

        public int ByteCount { get; }

        public static HttpStatusLine Parse(byte[] buffer, int start)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (start < 0 || start >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            var crLfIndex = buffer.IndexOfSubarray(CoreHelper.CrLfBytes, start, -1);
            if (crLfIndex == -1)
            {
                throw new BadHttpDataException("Could not parse status line");
            }

            // version
            var indexOfSpaceAfterVersion = buffer.IndexOf(SPACE_BYTE, start);
            if (indexOfSpaceAfterVersion == -1)
            {
                throw new BadHttpDataException("Could not parse status line");
            }

            var length = indexOfSpaceAfterVersion - start;

            if (length == 0)
            {
                // version length is 0
                throw new BadHttpDataException("Could not parse status line"); // todo1[ak] a lot of copy/paste
            }

            var version = buffer.ToAsciiString(start, length);

            // advance
            start = indexOfSpaceAfterVersion + 1; // skip ' '

            // code
            var indexOfSpaceAfterCode = buffer.IndexOf(SPACE_BYTE, start);

            length = indexOfSpaceAfterCode - start;
            if (length == 0)
            {
                // code length is 0
                throw new BadHttpDataException("Could not parse status line");
            }

            var codeNumber = buffer.ToAsciiString(start, length).ToInt32();
            var code = (HttpStatusCode)codeNumber;

            // advance
            start = indexOfSpaceAfterCode + 1; // skip ' '

            // reason
            length = crLfIndex - start;
            if (length == 0)
            {
                // reason length is 0
                throw new BadHttpDataException("Could not parse status line");
            }

            var reason = buffer.ToAsciiString(start, length);
            if (reason[0] == ' ')
            {
                // reason must not start with space
                throw new BadHttpDataException("Could not parse status line");
            }

            try
            {
                var line = new HttpStatusLine(version, code, reason);
                return line;
            }
            catch (ArgumentException ex)
            {
                throw new BadHttpDataException("Could not parse request line", ex);
            }
        }
    }
}
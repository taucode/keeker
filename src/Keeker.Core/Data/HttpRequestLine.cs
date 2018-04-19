using Keeker.Core.Exceptions;
using System;
using System.Net.Http;
using System.Text;

namespace Keeker.Core.Data
{
    public class HttpRequestLine
    {
        private const byte SPACE_BYTE = (byte)' ';

        public HttpRequestLine(HttpMethod method, string requestUri, string version)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var methodIsValid =
                ReferenceEquals(method, HttpMethod.Get) ||
                ReferenceEquals(method, HttpMethod.Put) ||
                ReferenceEquals(method, HttpMethod.Post) ||
                ReferenceEquals(method, HttpMethod.Delete) ||
                ReferenceEquals(method, HttpMethod.Head) ||
                ReferenceEquals(method, HttpMethod.Options) ||
                ReferenceEquals(method, HttpMethod.Trace) ||
                CoreHelper.IsValidHttpMethod(method.Method);

            if (!methodIsValid)
            {
                throw new ArgumentException("Invalid HTTP method", nameof(method));
            }

            if (requestUri == null)
            {
                throw new ArgumentNullException(nameof(requestUri));
            }

            if (!CoreHelper.IsValidUri(requestUri))
            {
                throw new ArgumentException("Invalid URI", nameof(requestUri));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (!CoreHelper.IsValidHttpVersion(version))
            {
                throw new ArgumentException("Invalid HTTP version", nameof(version));
            }

            this.Method = method;
            this.RequestUri = requestUri;
            this.Version = version;

            this.ByteCount =
                this.Method.ToString().Length + 1 +
                this.RequestUri.Length + 1 +
                this.Version.Length + CoreHelper.CrLfBytes.Length;
        }

        public HttpRequestLine(HttpMethod method, string requestUri)
            : this(method, requestUri, CoreHelper.HttpVersion11)
        {
        }

        public HttpMethod Method { get; }
        public string RequestUri { get; }
        public string Version { get; }

        public override string ToString()
        {
            return $"{this.Method} {this.RequestUri} {this.Version}{CoreHelper.CrLf}";
        }

        public byte[] ToArray()
        {
            return Encoding.ASCII.GetBytes(this.ToString());
        }

        public int ByteCount { get; }

        public static HttpRequestLine Parse(byte[] buffer, int start)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            var crLfIndex = buffer.IndexOfSubarray(CoreHelper.CrLfBytes, start, -1);
            if (crLfIndex == -1)
            {
                throw new BadHttpDataException();
            }

            // method
            var indexOfSpaceAfterMethod = buffer.IndexOf(SPACE_BYTE, start);
            if (indexOfSpaceAfterMethod == -1)
            {
                throw new BadHttpDataException();
            }

            var length = indexOfSpaceAfterMethod - start;
            var method = buffer.ToAsciiString(start, length);

            // advance
            start = indexOfSpaceAfterMethod + 1; // skip ' '

            // uri
            var indexOfSpaceAfterUri = buffer.IndexOf(SPACE_BYTE, start);

            length = indexOfSpaceAfterUri - start;
            var uri = buffer.ToAsciiString(start, length);

            // advance
            start = indexOfSpaceAfterUri + 1; // skip ' '

            // http version
            length = crLfIndex - start;
            var version = buffer.ToAsciiString(start, length);

            try
            {
                var line = new HttpRequestLine(new HttpMethod(method), uri, version);

                return line;
            }
            catch (ArgumentException ex)
            {
                throw new BadHttpDataException("Could not parse request line", ex);
            }
        }
    }
}

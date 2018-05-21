using Keeker.Core.Data;
using Keeker.Core.Data.Builders;
using Keeker.Core.Listeners;
using Keeker.Core.Streams;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Keeker.Core
{
    public static class CoreHelper
    {
        public static byte[] PeekAll(this ByteAccumulator accumulator)
        {
            var count = accumulator.Count;
            var buffer = new byte[count];

            accumulator.Peek(buffer, 0, count);
            return buffer;
        }

        public static int IndexOfSubarray<T>(this T[] array, T[] subarray, int start, int length = -1)
            where T : IEquatable<T>
        {
            const int NOT_FOUND = -1;
            const int DEFAULT_LENGTH = -1;

            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (subarray == null)
            {
                throw new ArgumentNullException(nameof(subarray));
            }

            if (subarray.Length == 0)
            {
                throw new ArgumentException("Subarray cannot be empty", nameof(subarray));
            }

            if (length == DEFAULT_LENGTH)
            {
                length = array.Length;
            }
            else if (length < 0)
            {
                throw new ArgumentException("Bad value for length. To default to array's length, use value -1.", nameof(length));
            }
            else if (length == 0)
            {
                // 0-length array doesn't contain any non-empty subarray
                return NOT_FOUND;
            }

            if (length > array.Length)
            {
                throw new ArgumentException("length cannot be larger than array's Length.");
            }

            var curr = start;
            var subarrayLen = subarray.Length;

            while (true)
            {
                if (curr + subarrayLen > length)
                {
                    return NOT_FOUND;
                }

                for (int i = 0; i < subarrayLen; i++)
                {
                    var arrayIndex = curr + i;

                    var arrayElement = array[arrayIndex];
                    var subarrayElement = subarray[i];

                    if (!arrayElement.Equals(subarrayElement))
                    {
                        break; // no luck
                    }

                    if (i == subarrayLen - 1)
                    {
                        // all equal; return!
                        return curr;
                    }
                }

                curr++;
            }
        }

        public static byte[] ToAsciiBytes(this string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }

        public static string ToAsciiString(this byte[] bytes, int start, int count)
        {
            var s = Encoding.ASCII.GetString(bytes, start, count);
            return s;
        }

        public static string ToAsciiString(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        public static int ToInt32(this string s)
        {
            return int.Parse(s, CultureInfo.InvariantCulture);
        }

        public static IPEndPoint ToIPEndPoint(this string s)
        {
            var parts = s.Split(':');
            var address = IPAddress.Parse(parts[0]);
            var port = parts[1].ToInt32();
            return new IPEndPoint(address, port);
        }

        public static int ToIn32FromHex(this string s)
        {
            var n = int.Parse(s, NumberStyles.HexNumber);
            return n;
        }

        public static TEnum ToEnum<TEnum>(this string s)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), s, true);
        }

        public static void WriteAll(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void RedirectStream(ManualResetEvent signal, Stream source, Stream destination, byte[] buffer, int length)
        {
            var remaining = length;

            while (true)
            {
                // todo0000[ak] avoid 100%, here and in other 'while (true)'

                var gotSignal = signal.WaitOne(0);
                if (gotSignal)
                {
                    break;
                }

                if (remaining == 0)
                {
                    break;
                }

                var portionSize = Math.Min(remaining, buffer.Length);
                var bytesReadCount = source.Read(buffer, 0, portionSize);
                destination.Write(buffer, 0, bytesReadCount);

                remaining -= bytesReadCount;
            }
        }

        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IList<T> items, Func<T, bool> predicate, int start = 0)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            for (int i = start; i < items.Count; i++)
            {
                var item = items[i];
                if (predicate(item)) return i;
            }

            return -1;
        }

        ///<summary>Finds the index of the first occurrence of an item in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="item">The item to find.</param>
        ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf<T>(this IList<T> items, T item, int length = 0)
        {
            return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i), length);
        }

        public static bool ByteArraysEquivalent(
            byte[] array1,
            int start1,
            byte[] array2,
            int start2,
            int length)
        {
            // todo1[ak] checks

            for (int i = 0; i < length; i++)
            {
                var index1 = start1 + i;
                if (index1 >= array1.Length)
                {
                    return false;
                }

                var index2 = start2 + i;
                if (index2 >= array2.Length)
                {
                    return false;
                }

                var b1 = array1[index1];
                var b2 = array2[index2];

                if (b1 != b2)
                {
                    return false;
                }
            }

            return true;
        }

        #region HTTP

        public static class ContentTypes
        {
            public const string TextHtml = "text/html";
        }

        public const int DEFAULT_HTTPS_PORT = 443;
        public const int DEFAULT_HTTP_PORT = 80;

        public const string HttpVersion11 = "HTTP/1.1";

        public const string CrLf = "\r\n";
        public static readonly byte[] CrLfBytes = CrLf.ToAsciiBytes();
        public const string CrLfCrLf = CrLf + CrLf;
        public static readonly byte[] CrLfCrLfBytes = CrLfCrLf.ToAsciiBytes();

        public static string GetLocation(this HttpHeaderCollection httpHeaders)
        {
            return httpHeaders.Single(x => x.Name == "Location").Value;
        }

        public static string GetHost(this HttpHeaderCollection httpHeaders)
        {
            return httpHeaders.Single(x => x.Name == "Host").Value;
        }

        public static int GetContentLength(this HttpHeaderCollection httpHeaders)
        {
            return httpHeaders.Single(x => x.Name == "Content-Length").Value.ToInt32();
        }

        public static HttpTransferEncoding GetTransferEncoding(this HttpHeaderCollection httpHeaders)
        {
            return httpHeaders.Single(x => x.Name == "Transfer-Encoding").Value.ToEnum<HttpTransferEncoding>();
        }

        public static string ToInfoString(this Socket socket, bool includeRemoteEndpoint = false)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Handle: {0:x8}", socket.Handle.ToInt32());
            sb.AppendFormat(" L: {0}", socket.LocalEndPoint);

            if (includeRemoteEndpoint)
            {
                sb.AppendFormat(" R: {0}", socket.RemoteEndPoint);
            }

            var res = sb.ToString();
            return res;
        }

        public static string ToReason(this HttpStatusCode code)
        {
            switch (code)
            {
                case HttpStatusCode.Continue:
                    return "Continue";

                case HttpStatusCode.SwitchingProtocols:
                    return "Switching Protocols";

                case HttpStatusCode.OK:
                    return "OK";

                case HttpStatusCode.Created:
                    return "Created";

                case HttpStatusCode.Accepted:
                    return "Accepted";

                case HttpStatusCode.NonAuthoritativeInformation:
                    return "Non-Authoritative Information";

                case HttpStatusCode.NoContent:
                    return "No Content";

                case HttpStatusCode.ResetContent:
                    return "Reset Content";

                case HttpStatusCode.PartialContent:
                    return "Partial Content";

                case HttpStatusCode.MultipleChoices:
                    return "Multiple Choices";

                case HttpStatusCode.MovedPermanently:
                    return "Moved Permanently";

                case HttpStatusCode.Found:
                    return "Found";

                case HttpStatusCode.SeeOther:
                    return "See Other";

                case HttpStatusCode.NotModified:
                    return "Not Modified";

                case HttpStatusCode.UseProxy:
                    return "Use Proxy";

                case HttpStatusCode.Unused:
                    return "Unused";

                case HttpStatusCode.TemporaryRedirect:
                    return "Temporary Redirect";

                case HttpStatusCode.BadRequest:
                    return "Bad Request";

                case HttpStatusCode.Unauthorized:
                    return "Unauthorized";

                case HttpStatusCode.PaymentRequired:
                    return "Payment Required";

                case HttpStatusCode.Forbidden:
                    return "Forbidden";

                case HttpStatusCode.NotFound:
                    return "Not Found";

                case HttpStatusCode.MethodNotAllowed:
                    return "Method Not Allowed";

                case HttpStatusCode.NotAcceptable:
                    return "Not Acceptable";

                case HttpStatusCode.ProxyAuthenticationRequired:
                    return "Proxy Authentication Required";

                case HttpStatusCode.RequestTimeout:
                    return "Request Timeout";

                case HttpStatusCode.Conflict:
                    return "Conflict";

                case HttpStatusCode.Gone:
                    return "Gone";

                case HttpStatusCode.LengthRequired:
                    return "Length Required";

                case HttpStatusCode.PreconditionFailed:
                    return "Precondition Failed";

                case HttpStatusCode.RequestEntityTooLarge:
                    return "Payload Too Large";

                case HttpStatusCode.RequestUriTooLong:
                    return "URI Too Long";

                case HttpStatusCode.UnsupportedMediaType:
                    return "Unsupported Media Type";

                case HttpStatusCode.RequestedRangeNotSatisfiable:
                    return "Range Not Satisfiable";

                case HttpStatusCode.ExpectationFailed:
                    return "Expectation Failed";

                case HttpStatusCode.UpgradeRequired:
                    return "Upgrade Required";

                case HttpStatusCode.InternalServerError:
                    return "Internal Server Error";

                case HttpStatusCode.NotImplemented:
                    return "Not Implemented";

                case HttpStatusCode.BadGateway:
                    return "Bad Gateway";

                case HttpStatusCode.ServiceUnavailable:
                    return "Service Unavailable";

                case HttpStatusCode.GatewayTimeout:
                    return "Gateway Timeout";

                case HttpStatusCode.HttpVersionNotSupported:
                    return "HTTP Version Not Supported";

                default:
                    return "Unknown";
            }
        }

        public static HttpHeaderCollectionBuilder SetContentType(
            this HttpHeaderCollectionBuilder builder,
            string contentType)
        {
            builder.Replace("Content-Type", contentType);
            return builder;
        }

        public static HttpHeaderCollectionBuilder SetContentLength(
            this HttpHeaderCollectionBuilder builder,
            int contentLength)
        {
            builder.Replace("Content-Length", contentLength.ToString());
            return builder;
        }

        private static readonly HashSet<char> ValidHeaderNameChars;
        private static readonly HashSet<char> ValidHeaderValueChars;
        private static readonly HashSet<char> ValidHttpMethodChars;
        private static readonly HashSet<char> ValidUriChars;
        private static readonly HashSet<char> ValidHttpVersionChars;
        private static readonly HashSet<char> ValidStatusReasonChars;

        static CoreHelper()
        {
            ValidHeaderNameChars = new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-");

            var validHeaderValueChars = Enumerable.Range(0x20, 128 - 0x20 - 1).Select(n => (char)n);
            ValidHeaderValueChars = new HashSet<char>(validHeaderValueChars);

            ValidHttpMethodChars = new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            ValidUriChars = new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~:/?#[]@!$&'()*+,;=`%");

            ValidHttpVersionChars = new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789./");

            ValidStatusReasonChars = new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz -");
        }

        public static bool IsValidHeaderValue(string value)
        {
            if (value == null)
            {
                return false;
            }

            if (value.Length == 0)
            {
                return true; // RFC allows empty header values
            }

            return value.All(c => ValidHeaderValueChars.Contains(c));
        }

        public static bool IsValidHeaderName(string name)
        {
            if (name == null)
            {
                return false;
            }

            if (name.Length == 0)
            {
                return false;
            }

            return name.All(c => ValidHeaderNameChars.Contains(c));
        }

        public static bool IsValidHttpMethod(string method)
        {
            if (method == null)
            {
                return false;
            }

            return method.Length != 0 && method.All(c => ValidHttpMethodChars.Contains(c));
        }

        public static bool IsValidUri(string uri)
        {
            if (uri == null)
            {
                return false;
            }

            return uri.Length != 0 && uri.All(c => ValidUriChars.Contains(c));
        }

        public static bool IsValidHttpVersion(string version)
        {
            if (version == null)
            {
                return false;
            }

            return version.Length != 0 && version.All(c => ValidHttpVersionChars.Contains(c));
        }

        public static bool IsValidStatusReason(string reason)
        {
            if (reason == null)
            {
                return false;
            }

            return reason.Length != 0 && reason.All(c => ValidStatusReasonChars.Contains(c));
        }

        #endregion

        public static IPEndPoint TryParseIPEndPoint(string endPoint)
        {
            if (endPoint == null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            var m = Regex.Match(endPoint, @"tcp://(\d+\.\d+\.\d+\.\d+):(\d+)");
            if (!m.Success)
            {
                return null;
            }

            throw new NotImplementedException();
        }

        public static bool IsIPEndPoint(string endPoint)
        {
            return TryParseIPEndPoint(endPoint) != null;
        }

        public static int? TryParseLinkEndPoint(string endPoint)
        {
            if (endPoint == null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            var m = Regex.Match(endPoint, @"link://(\d+)");
            if (!m.Success)
            {
                return null;
            }

            return m.Result("$1").ToInt32();
        }

        public static bool IsLinkEndPoint(string endPoint)
        {
            return TryParseLinkEndPoint(endPoint) != null;
        }

        public static IStreamListener CreateListenerForEndPoint(string endPoint)
        {
            if (IsLinkEndPoint(endPoint))
            {
                return new LinkStreamListener(TryParseLinkEndPoint(endPoint).Value);
            }
            else if (IsIPEndPoint(endPoint))
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new ApplicationException();
            }
        }

        public static Stream CreateStreamFromEndPoint(string endPoint)
        {
            if (IsLinkEndPoint(endPoint))
            {
                var port = TryParseLinkEndPoint(endPoint).Value;
                var link = new Link();
                LinkStreamListener.Connect(port, link);
                return link.Stream1;
            }
            else if (IsIPEndPoint(endPoint))
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new ApplicationException();
            }
        }
    }
}

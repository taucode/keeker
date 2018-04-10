using Keeker.Convey.Data;
using Keeker.Convey.Streams;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Keeker.Convey
{
    internal static class Helper
    {
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
            var n = Int32.Parse(s, System.Globalization.NumberStyles.HexNumber);
            return n;
        }

        public static TEnum ToEnum<TEnum>(this string s)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), s, true);
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

        public static byte[] PeekAll(this ByteAccumulator accumulator)
        {
            var count = accumulator.Count;
            var buffer = new byte[count];

            accumulator.Peek(buffer, 0, count);
            return buffer;
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

        internal const int DEFAULT_HTTPS_PORT = 443;
        internal const int DEFAULT_HTTP_PORT = 80;

        internal const string CrLf = "\r\n";
        internal static readonly byte[] CrLfBytes = CrLf.ToAsciiBytes();
        internal const string CrLfCrLf = CrLf + CrLf;
        internal static readonly byte[] CrLfCrLfBytes = CrLfCrLf.ToAsciiBytes();

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

        #endregion
    }
}

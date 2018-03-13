using Keeker.Core.Conf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Keeker.Core
{
    public static class CoreHelper
    {
        public static ProxyPlainConf ToProxyPlainConf(this ProxySection proxySection)
        {
            var plainKeekerSection = new ProxyPlainConf
            {
                Certificates = proxySection.Certificates?
                    .Cast<CertificateElement>()
                    .ToDictionary(
                        x => x.Id,
                        x => x.ToCertificatePlainConf()),

                Listeners = proxySection.Listeners?
                    .Cast<ListenerElement>()
                    .ToDictionary(
                        x => x.Id,
                        x => x.ToListenerPlainConf())
            };

            plainKeekerSection.Listeners = plainKeekerSection.Listeners ?? new Dictionary<string, ListenerPlainConf>();

            return plainKeekerSection;
        }

        public static ProxyPlainConf Clone(this ProxyPlainConf conf)
        {
            var clone = new ProxyPlainConf
            {
                Certificates = conf.Certificates?
                    .ToDictionary(x => x.Key, x => x.Value.Clone()),

                Listeners = conf.Listeners?
                    .ToDictionary(x => x.Key, x => x.Value.Clone()),
            };

            clone.Listeners = clone.Listeners ?? new Dictionary<string, ListenerPlainConf>();
            return clone;
        }

        public static ListenerPlainConf ToListenerPlainConf(this ListenerElement listenerElement)
        {
            var res = new ListenerPlainConf
            {
                Id = listenerElement.Id,
                EndPoint = listenerElement.EndPoint.ToIPEndPoint(),
                IsHttps = listenerElement.IsHttps,
                Hosts = listenerElement.Hosts
                    .Cast<HostElement>()
                    .Select(ToHostPlainConf)
                    .ToDictionary(x => x.ExternalHostName, x => x),
            };

            return res;
        }

        public static ListenerPlainConf Clone(this ListenerPlainConf conf)
        {
            return new ListenerPlainConf
            {
                Id = conf.Id,
                EndPoint = conf.EndPoint.CloneEndPoint(),
                IsHttps = conf.IsHttps,
                Hosts = conf.Hosts.Values
                    .Select(Clone)
                    .ToDictionary(x => x.ExternalHostName, x => x),
            };
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
                
        public static HostPlainConf ToHostPlainConf(this HostElement hostElement)
        {
            var res = new HostPlainConf
            {
                ExternalHostName = hostElement.ExternalHostName,
                DomesticHostName = hostElement.DomesticHostName,
                EndPoint = hostElement.EndPoint.ToIPEndPoint(),
                CertificateId = hostElement.CertificateId,
            };

            return res;
        }

        public static HostPlainConf Clone(this HostPlainConf conf)
        {
            return new HostPlainConf
            {
                ExternalHostName = conf.ExternalHostName,
                DomesticHostName = conf.DomesticHostName,
                EndPoint = conf.EndPoint.CloneEndPoint(),
                CertificateId = conf.CertificateId,
            };
        }

        public static CertificatePlainConf ToCertificatePlainConf(this CertificateElement certificateElement)
        {
            var res = new CertificatePlainConf
            {
                Id = certificateElement.Id,
                FilePath = certificateElement.FilePath,
                Password = certificateElement.Password,
                Domains = certificateElement.Domains
                    .Cast<DomainElement>()
                    .Select(x => x.Name)
                    .ToHashSet(),
            };

            return res;
        }

        public static CertificatePlainConf Clone(this CertificatePlainConf conf)
        {
            return new CertificatePlainConf
            {
                Id = conf.Id,
                FilePath = conf.FilePath,
                Password = conf.Password,
                Domains = conf.Domains.ToHashSet(),
            };
        }
        
        public static IPEndPoint CloneEndPoint(this IPEndPoint endPoint)
        {
            return new IPEndPoint(endPoint.Address, endPoint.Port);
        }

        public static byte[] ToAsciiBytes(this string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }

        public static string ToAsciiString(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        public static string ToAsciiString(this byte[] bytes, int start, int count)
        {
            var s = Encoding.ASCII.GetString(bytes, start, count);
            return s;
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

        //public static void PutEntireSegment(this ByteAccumulator accumulator, byte[] segment)
        //{
        //    accumulator.Put(segment, 0, segment.Length);
        //}

        public static byte[] PeekAll(this ByteAccumulator accumulator)
        {
            var count = accumulator.Count;
            var buffer = new byte[count];

            accumulator.Peek(buffer, 0, count);
            return buffer;
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

        public static void WriteAll(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }

        public static int ToInt32(this string s)
        {
            return int.Parse(s, CultureInfo.InvariantCulture);
        }

        public static int ToIn32FromHex(this string s)
        {
            var n = Int32.Parse(s, System.Globalization.NumberStyles.HexNumber);
            return n;
        }

        public static IPEndPoint ToIPEndPoint(this string s)
        {
            var parts = s.Split(':');
            var address = IPAddress.Parse(parts[0]);
            var port = parts[1].ToInt32();
            return new IPEndPoint(address, port);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection, bool checkUniqueness = true)
        {
            var hashSet = checkUniqueness
                ? new HashSet<T>(collection.ToDictionary(x => x, x => (byte)0).Keys)
                : new HashSet<T>(collection);

            return hashSet;
        }

        public static string[] GetUserCertificateIds(this ListenerPlainConf conf)
        {
            return conf.Hosts?
                .Select(x => x.Value.CertificateId)
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .ToArray();
        }

        public static TEnum ToEnum<TEnum>(this string s)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), s, true);
        }
    }
}

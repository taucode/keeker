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
                Relays = listenerElement.Relays
                    .Cast<RelayElement>()
                    .Select(ToRelayPlainConf)
                    .ToList(),
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
                Relays = conf.Relays
                    .Select(Clone)
                    .ToList(),
            };
        }

        //public static HostPlainConf ToHostPlainConf(this HostElement hostElement)
        //{
        //    var res = new HostPlainConf
        //    {
        //        ExternalHostName = hostElement.ExternalHostName,
        //        Relay = hostElement.Relay.ToRelayPlainConf(),
        //        Certificate = hostElement.Certificate.ToCertificatePlainConf(),
        //    };

        //    return res;
        //}

        //public static HostPlainConf Clone(this HostPlainConf conf)
        //{
        //    return new HostPlainConf
        //    {
        //        ExternalHostName = conf.ExternalHostName,
        //        Relay = conf.Relay.Clone(),
        //        Certificate = conf.Certificate.Clone(),
        //    };
        //}

        public static RelayPlainConf ToRelayPlainConf(this RelayElement relayElement)
        {
            var res = new RelayPlainConf
            {
                ExternalHostName = relayElement.ExternalHostName,
                DomesticHostName = relayElement.DomesticHostName,
                EndPoint = relayElement.EndPoint.ToIPEndPoint(),
                CertificateId = relayElement.CertificateId,
            };

            return res;
        }

        public static RelayPlainConf Clone(this RelayPlainConf conf)
        {
            return new RelayPlainConf
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

        //public static IPEndPoint GetEndPoint(this ListenerPlainConf conf)
        //{
        //    return new IPEndPoint(conf.Address, conf.Port);
        //}

        //public static IPEndPoint GetEndPoint(this RelayPlainConf conf)
        //{
        //    return new IPEndPoint(conf.Address, conf.Port);
        //}

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

        public static string GetAsciiSubstring(this byte[] bytes, int index, int count)
        {
            var s = Encoding.ASCII.GetString(bytes, index, count);
            return s;
        }

        public static void WriteAll(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
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

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection, bool checkUniqueness = true)
        {
            var hashSet = checkUniqueness
                ? new HashSet<T>(collection.ToDictionary(x => x, x => (byte)0).Keys)
                : new HashSet<T>(collection);

            return hashSet;
        }

        public static string[] GetUserCertificateIds(this ListenerPlainConf conf)
        {
            return conf.Relays?
                .Select(x => x.CertificateId)
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .ToArray();
        }
    }
}

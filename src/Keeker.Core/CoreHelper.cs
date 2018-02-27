using System;
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
                Address = IPAddress.Parse(proxySection.Address),
                Port = proxySection.Port,
                Hosts = proxySection.Hosts
                    .Cast<HostElement>()
                    .ToDictionary(
                        x => x.ExternalHostName,
                        x => new ProxyPlainConf.HostEntry
                        {
                            ExternalHostName = x.ExternalHostName,
                            Targets = x.Targets
                                .Cast<TargetElement>()
                                .Select(y => new ProxyPlainConf.TargetEntry
                                {
                                    Id = y.Id,
                                    IsActive = y.IsActive,
                                    DomesticHostName = y.DomesticHostName,
                                    Address = IPAddress.Parse(y.Address),
                                    Port = y.Port,
                                })
                                .ToArray(),
                            Certificate = new ProxyPlainConf.Certificate
                            {
                                FilePath = x.Certificate.FilePath,
                                Password = x.Certificate.Password,
                            },
                        }),
            };

            return plainKeekerSection;
        }

        public static IPEndPoint ClonEndPoint(this IPEndPoint endPoint)
        {
            return new IPEndPoint(endPoint.Address, endPoint.Port);
        }

        public static byte[] ToAsciiBytes(this string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }

        public static int IndexOfSubarray<T>(this T[] array, T[] subarray, int start, int length) 
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
            else if (length <= 0)
            {
                throw new ArgumentException("Bad value for length. To default to array's length, use value -1.", nameof(length));
            }

            if (length > array.Length)
            {
                throw new ArgumentException("length cannot be larger than array's Length.");
            }

            var curr = start;
            //var arrayLen = array.Length;
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

        public static void PutEntireSegment(this ByteAccumulator accumulator, byte[] segment)
        {
            accumulator.Put(segment, 0, segment.Length);
        }
    }
}

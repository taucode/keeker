using System.Linq;
using System.Net;

namespace Keeker.Core
{
    public static class CoreHelper
    {
        public static ProxyPlainConf ToPlainKeekerSection(this ProxySection keekerSection)
        {
            var plainKeekerSection = new ProxyPlainConf
            {
                IpAddress = IPAddress.Parse(keekerSection.IpAddress),
                Port = keekerSection.Port,
                Hosts = keekerSection.Hosts
                    .Cast<HostElement>()
                    .ToDictionary(
                        x => x.Name,
                        x => new ProxyPlainConf.HostEntry
                        {
                            Name = x.Name,
                            Targets = x.Targets
                                .Cast<TargetElement>()
                                .Select(y => new ProxyPlainConf.TargetEntry
                                {
                                    Name = y.Name,
                                    IsActive = y.IsActive,
                                    Host = y.Host,
                                    IpAddress = IPAddress.Parse(y.IpAddress),
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
    }
}

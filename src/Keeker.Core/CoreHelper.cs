using System.Linq;
using System.Net;

namespace Keeker.Core
{
    public static class CoreHelper
    {
        public static PlainKeekerSection ToPlainKeekerSection(this KeekerSection keekerSection)
        {
            var plainKeekerSection = new PlainKeekerSection
            {
                IpAddress = IPAddress.Parse(keekerSection.IpAddress),
                Port = keekerSection.Port,
                Hosts = keekerSection.Hosts
                    .Cast<HostElement>()
                    .ToDictionary(
                        x => x.Name,
                        x => new PlainKeekerSection.HostEntry
                        {
                            Name = x.Name,
                            Targets = x.Targets
                                .Cast<TargetElement>()
                                .Select(y => new PlainKeekerSection.TargetEntry
                                {
                                    Name = y.Name,
                                    IsActive = y.IsActive,
                                    Host = y.Host,
                                    IpAddress = IPAddress.Parse(y.IpAddress),
                                    Port = y.Port,
                                })
                                .ToArray(),
                            Certificate = new PlainKeekerSection.Certificate
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

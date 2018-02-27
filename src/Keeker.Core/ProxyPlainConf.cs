using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace Keeker.Core
{
    public class ProxyPlainConf
    {
        public IPAddress Address { get; set; }
        public int Port { get; set; }

        public class HostEntry
        {
            public string ExternalHostName { get; set; }
            public TargetEntry[] Targets { get; set; }
            public Certificate Certificate { get; set; }

            internal HostEntry Clone()
            {
                return new HostEntry
                {
                    ExternalHostName = this.ExternalHostName,
                    Targets = this.Targets?
                        .Select(x => x.Clone())
                        .ToArray(),
                    Certificate = this.Certificate.Clone(),
                };
            }
        }

        public class TargetEntry
        {
            public string Id { get; set; }
            public bool IsActive { get; set; }
            public string DomesticHostName { get; set; }
            public IPAddress Address { get; set; }
            public int Port { get; set; }

            internal TargetEntry Clone()
            {
                return new TargetEntry
                {
                    Id = this.Id,
                    IsActive = this.IsActive,
                    DomesticHostName = this.DomesticHostName,
                    Address = this.Address,
                    Port = this.Port,
                };
            }
        }

        public class Certificate
        {
            public string FilePath { get; set; }
            public string Password { get; set; }

            internal Certificate Clone()
            {
                return new Certificate
                {
                    FilePath = this.FilePath,
                    Password = this.Password,
                };
            }
        }

        public Dictionary<string, HostEntry> Hosts { get; set; }

        public static ProxyPlainConf LoadFromAppConfig(string sectionName)
        {
            var section = ConfigurationManager.GetSection(sectionName) as ProxySection;
            if (section == null)
            {
                throw new InvalidOperationException($"Could not find section '{sectionName}', or the section is invalid");
            }

            return section.ToProxyPlainConf();
        }

        internal ProxyPlainConf Clone()
        {
            return new ProxyPlainConf
            {
                Address = this.Address,
                Port = this.Port,
                Hosts = this.Hosts?
                    .ToDictionary(p => p.Key, p => p.Value.Clone()),
            };
        }
    }
}

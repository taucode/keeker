using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace Keeker.Core
{
    public class ProxyPlainConf
    {
        public Dictionary<string, ListenerPlainConf> Listeners { get; set; }

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
                Listeners = this.Listeners?
                    .ToDictionary(x => x.Key, x => x.Value.Clone()),
            };
        }
    }

    public class ListenerPlainConf
    {
        public string Id { get; set; }
        public IPAddress Address { get; set; }
        public int Port { get; set; }
        public Dictionary<string, HostPlainConf> Hosts { get; set; }

        internal ListenerPlainConf Clone()
        {
            return new ListenerPlainConf
            {
                Id = this.Id,
                Address = this.Address,
                Port = this.Port,
                Hosts = this.Hosts?
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value.Clone()),
            };
        }
    }

    public class HostPlainConf
    {
        public string ExternalHostName { get; set; }
        public TargetPlainConf[] Targets { get; set; }
        public CertificatePlainConf Certificate { get; set; }

        internal HostPlainConf Clone()
        {
            return new HostPlainConf
            {
                ExternalHostName = this.ExternalHostName,
                Targets = this.Targets?
                    .Select(x => x.Clone())
                    .ToArray(),
                Certificate = this.Certificate.Clone(),
            };
        }
    }

    public class TargetPlainConf
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string DomesticHostName { get; set; }
        public IPAddress Address { get; set; }
        public int Port { get; set; }

        internal TargetPlainConf Clone()
        {
            return new TargetPlainConf
            {
                Id = this.Id,
                IsActive = this.IsActive,
                DomesticHostName = this.DomesticHostName,
                Address = this.Address,
                Port = this.Port,
            };
        }
    }

    public class CertificatePlainConf
    {
        public string FilePath { get; set; }
        public string Password { get; set; }

        internal CertificatePlainConf Clone()
        {
            return new CertificatePlainConf
            {
                FilePath = this.FilePath,
                Password = this.Password,
            };
        }
    }
}

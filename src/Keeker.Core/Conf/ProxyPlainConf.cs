using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace Keeker.Core.Conf
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
    }

    public class ListenerPlainConf
    {
        public string Id { get; set; }
        public IPAddress Address { get; set; }
        public int Port { get; set; }
        public bool IsHttps { get; set; }

        public Dictionary<string, HostPlainConf> Hosts { get; set; }
    }

    public class HostPlainConf
    {
        public string ExternalHostName { get; set; }
        public RelayPlainConf Relay { get; set; }
        public CertificatePlainConf Certificate { get; set; }
    }

    public class RelayPlainConf
    {
        public string DomesticHostName { get; set; }
        public IPAddress Address { get; set; }
        public int Port { get; set; }
    }

    public class CertificatePlainConf
    {
        public string FilePath { get; set; }
        public string Password { get; set; }
    }
}

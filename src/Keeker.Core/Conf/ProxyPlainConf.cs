using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace Keeker.Core.Conf
{
    public class ProxyPlainConf
    {
        public Dictionary<string, CertificatePlainConf> Certificates { get; set; }
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

    public class CertificatePlainConf
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public string Password { get; set; }
    }

    public class ListenerPlainConf
    {
        public string Id { get; set; }
        public IPEndPoint EndPoint { get; set; }
        public bool IsHttps { get; set; }
        public List<RelayPlainConf> Relays { get; set; }
    }

    public class RelayPlainConf
    {
        public string ExternalHostName { get; set; }
        public string DomesticHostName { get; set; }
        public IPEndPoint EndPoint { get; set; }
        public string CertificateId { get; set; }
    }
}

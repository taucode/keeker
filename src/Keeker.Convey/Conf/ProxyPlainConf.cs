using Keeker.Core;
using System;
using System.Linq;
using System.Net;

namespace Keeker.Convey.Conf
{
    public class ProxyPlainConf
    {
        public ProxyPlainConf(
            CertificatePlainConf[] certificates,
            ListenerPlainConf[] listeners)
        {
            this.Certificates = certificates
                .Select(x => x.Clone())
                .ToArray();

            this.Listeners = listeners
                .Select(x => x.Clone())
                .ToArray();
        }

        public CertificatePlainConf[] Certificates { get; }

        public ListenerPlainConf[] Listeners { get; }
    }

    public class CertificatePlainConf
    {
        public string Id { get; }
        public string FilePath { get; }
        public string Password { get; }
        public DomainPlainConf[] Domains { get; }

        public CertificatePlainConf(
            string id,
            string filePath,
            string password,
            DomainPlainConf[] domains)
        {
            this.Id = id;
            this.FilePath = filePath;
            this.Password = password;
            this.Domains = domains;
        }
    }

    public class DomainPlainConf
    {
        public string Name { get; }

        public DomainPlainConf(string name)
        {
            this.Name = name;
        }
    }

    public class ListenerPlainConf
    {
        public string Id { get; }
        public IPEndPoint IPEndPoint { get; }
        public bool IsHttps { get; }
        public HostPlainConf[] Hosts { get; }

        public ListenerPlainConf(
            string id,
            IPEndPoint iPEndPoint,
            bool isHttps,
            HostPlainConf[] hosts)
        {
            this.Id = id;
            this.IPEndPoint = iPEndPoint;
            this.IsHttps = isHttps;
            this.Hosts = hosts;
        }
    }

    public class HostPlainConf
    {
        public string ExternalHostName { get; }
        public string DomesticHostName { get; }
        public IPEndPoint EndPoint { get; }
        public string CertificateId { get; }

        public HostPlainConf(
            string externalHostName,
            string domesticHostName,
            IPEndPoint endPoint,
            string certificateId)
        {
            this.ExternalHostName = externalHostName;
            this.DomesticHostName = domesticHostName;
            this.EndPoint = endPoint;
            this.CertificateId = certificateId;
        }
    }

    public static class ProxyPlainConfExtensions
    {
        public static ProxyPlainConf ToPlainConf(this ProxySection section)
        {
            return new ProxyPlainConf(
                section.Certificates
                    .Cast<CertificateElement>()
                    .Select(x => x.ToPlainConf())
                    .ToArray(),
                section.Listeners
                    .Cast<ListenerElement>()
                    .Select(x => x.ToPlainConf())
                    .ToArray());
        }

        public static ProxyPlainConf Clone(this ProxyPlainConf conf)
        {
            throw new NotImplementedException();
        }

        public static CertificatePlainConf ToPlainConf(this CertificateElement element)
        {
            return new CertificatePlainConf(
                element.Id,
                element.FilePath,
                element.Password,
                element.Domains
                    .Cast<DomainElement>()
                    .Select(x => x.ToPlainConf())
                    .ToArray());
        }

        public static CertificatePlainConf Clone(this CertificatePlainConf conf)
        {
            return new CertificatePlainConf(
                conf.Id,
                conf.FilePath,
                conf.Password,
                conf.Domains
                    .Select(x => x.Clone())
                    .ToArray());
        }

        public static DomainPlainConf ToPlainConf(this DomainElement element)
        {
            return new DomainPlainConf(element.Name);
        }

        public static DomainPlainConf Clone(this DomainPlainConf conf)
        {
            return new DomainPlainConf(conf.Name);
        }

        public static ListenerPlainConf ToPlainConf(this ListenerElement element)
        {
            return new ListenerPlainConf(
                element.Id,
                element.EndPoint.ToIPEndPoint(),
                element.IsHttps,
                element.Hosts
                    .Cast<HostElement>()
                    .Select(x => x.ToPlainConf())
                    .ToArray());
        }

        public static ListenerPlainConf Clone(this ListenerPlainConf conf)
        {
            return new ListenerPlainConf(
                conf.Id,
                conf.IPEndPoint,
                conf.IsHttps,
                conf.Hosts
                    .Select(x => x.Clone())
                    .ToArray());
        }

        public static HostPlainConf ToPlainConf(this HostElement element)
        {
            return new HostPlainConf(
                element.ExternalHostName,
                element.DomesticHostName,
                element.EndPoint.ToIPEndPoint(),
                element.CertificateId);
        }

        public static HostPlainConf Clone(this HostPlainConf conf)
        {
            return new HostPlainConf(
                conf.ExternalHostName,
                conf.DomesticHostName,
                conf.EndPoint,
                conf.CertificateId);
        }

        public static string[] GetUserCertificateIds(this ListenerPlainConf conf)
        {
            return conf.Hosts?
                .Select(x => x.CertificateId)
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .ToArray();
        }
    }
}

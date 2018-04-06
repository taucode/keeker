using System;
using System.Linq;
using System.Net;

namespace Keeker.Convey.Conf
{
    public class TauProxyPlainConf
    {
        public TauProxyPlainConf(
            TauCertificatePlainConf[] certificates,
            TauListenerPlainConf[] listeners)
        {
            this.Certificates = certificates
                .Select(x => x.Clone())
                .ToArray();

            this.Listeners = listeners
                .Select(x => x.Clone())
                .ToArray();
        }

        public TauCertificatePlainConf[] Certificates { get; }

        public TauListenerPlainConf[] Listeners { get; }
    }

    public class TauCertificatePlainConf
    {
        public string Id { get; }
        public string FilePath { get; }
        public string Password { get; }
        public TauDomainPlainConf[] Domains { get; }

        public TauCertificatePlainConf(
            string id,
            string filePath,
            string password,
            TauDomainPlainConf[] domains)
        {
            this.Id = id;
            this.FilePath = filePath;
            this.Password = password;
            this.Domains = domains;
        }
    }

    public class TauDomainPlainConf
    {
        public string Name { get; }

        public TauDomainPlainConf(string name)
        {
            this.Name = name;
        }
    }

    public class TauListenerPlainConf
    {
        public string Id { get; }
        public IPEndPoint IPEndPoint { get; }
        public bool IsHttps { get; }
        public TauHostPlainConf[] Hosts { get; }

        public TauListenerPlainConf(
            string id,
            IPEndPoint iPEndPoint,
            bool isHttps,
            TauHostPlainConf[] hosts)
        {
            this.Id = id;
            this.IPEndPoint = iPEndPoint;
            this.IsHttps = isHttps;
            this.Hosts = hosts;
        }
    }

    public class TauHostPlainConf
    {
        public string ExternalHostName { get; }
        public string DomesticHostName { get; }
        public IPEndPoint EndPoint { get; }
        public string CertificateId { get; }

        public TauHostPlainConf(
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

    public static class TauProxyPlainConfExtensions
    {
        public static TauProxyPlainConf ToPlainConf(this TauProxySection section)
        {
            return new TauProxyPlainConf(
                section.Certificates
                    .Cast<TauCertificateElement>()
                    .Select(x => x.ToPlainConf())
                    .ToArray(),
                section.Listeners
                    .Cast<TauListenerElement>()
                    .Select(x => x.ToPlainConf())
                    .ToArray());
        }

        public static TauProxyPlainConf Clone(this TauProxyPlainConf conf)
        {
            throw new NotImplementedException();
        }

        public static TauCertificatePlainConf ToPlainConf(this TauCertificateElement element)
        {
            return new TauCertificatePlainConf(
                element.Id,
                element.FilePath,
                element.Password,
                element.Domains
                    .Cast<TauDomainElement>()
                    .Select(x => x.ToPlainConf())
                    .ToArray());
        }

        public static TauCertificatePlainConf Clone(this TauCertificatePlainConf conf)
        {
            return new TauCertificatePlainConf(
                conf.Id,
                conf.FilePath,
                conf.Password,
                conf.Domains
                    .Select(x => x.Clone())
                    .ToArray());
        }

        public static TauDomainPlainConf ToPlainConf(this TauDomainElement element)
        {
            return new TauDomainPlainConf(element.Name);
        }

        public static TauDomainPlainConf Clone(this TauDomainPlainConf conf)
        {
            return new TauDomainPlainConf(conf.Name);
        }

        public static TauListenerPlainConf ToPlainConf(this TauListenerElement element)
        {
            return new TauListenerPlainConf(
                element.Id,
                element.EndPoint.ToIPEndPoint(),
                element.IsHttps,
                element.Hosts
                    .Cast<TauHostElement>()
                    .Select(x => x.ToPlainConf())
                    .ToArray());
        }

        public static TauListenerPlainConf Clone(this TauListenerPlainConf conf)
        {
            return new TauListenerPlainConf(
                conf.Id,
                conf.IPEndPoint,
                conf.IsHttps,
                conf.Hosts
                    .Select(x => x.Clone())
                    .ToArray());
        }

        public static TauHostPlainConf ToPlainConf(this TauHostElement element)
        {
            return new TauHostPlainConf(
                element.ExternalHostName,
                element.DomesticHostName,
                element.EndPoint.ToIPEndPoint(),
                element.CertificateId);
        }

        public static TauHostPlainConf Clone(this TauHostPlainConf conf)
        {
            return new TauHostPlainConf(
                conf.ExternalHostName,
                conf.DomesticHostName,
                conf.EndPoint,
                conf.CertificateId);
        }

        public static string[] GetUserCertificateIds(this TauListenerPlainConf conf)
        {
            return conf.Hosts?
                .Select(x => x.CertificateId)
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .ToArray();
        }

    }
}

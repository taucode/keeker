using System.Security.Cryptography.X509Certificates;

namespace Keeker.Core
{
    public class CertificateInfo
    {
        public CertificateInfo(string[] domainNames, X509Certificate2 certificate)
        {
            this.DomainNames = domainNames;
            this.Certificate = certificate;
        }

        public string[] DomainNames { get; }

        public X509Certificate2 Certificate { get; }
    }
}

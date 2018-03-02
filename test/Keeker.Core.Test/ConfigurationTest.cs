using Keeker.Core.Conf;
using NUnit.Framework;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using TauCode.Utils.Extensions;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class ConfigurationTest
    {
        private string _confFileName;

        [SetUp]
        public void SetUp()
        {
            _confFileName = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(_confFileName);
        }

        [Test]
        public void Configuration_IsValid_ReturnsExpectedResult()
        {
            // Arrange
            var proxySection = this.GetProxySection();

            // Act
            var conf = proxySection.ToProxyPlainConf();

            // Assert
            Assert.That(conf, Is.Not.Null);

            var certificates = conf.Certificates;
            Assert.That(certificates, Is.Not.Null);
            Assert.That(certificates, Has.Count.EqualTo(1));

            var certificate = certificates["rho"];
            Assert.That(certificate.Id, Is.EqualTo("rho"));
            Assert.That(certificate.FilePath, Is.EqualTo(@"c:\certs\rho.me.pfx"));
            Assert.That(certificate.Password, Is.EqualTo("doresaq1488"));
            CollectionAssert.AreEquivalent(new[] { "rho.me", }, certificate.Domains.ToArray());

            Assert.That(conf.Listeners, Is.Not.Null);
            Assert.That(conf.Listeners, Has.Count.EqualTo(2));

            // listener: sample
            var listenerConf = conf.Listeners["sample"];
            Assert.That(listenerConf.Id, Is.EqualTo("sample"));
            Assert.That(listenerConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:81"));
            Assert.That(listenerConf.IsHttps, Is.False);
            Assert.That(listenerConf.Relays, Is.Not.Null);
            Assert.That(listenerConf.Relays, Has.Count.EqualTo(2));

            // relay: www.rho.me
            var relayConf = listenerConf.Relays[0];
            Assert.That(relayConf.ExternalHostName, Is.EqualTo("www.rho.me"));
            Assert.That(relayConf.DomesticHostName, Is.EqualTo("localhost"));
            Assert.That(relayConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:53808"));
            Assert.That(string.IsNullOrEmpty(relayConf.CertificateId), Is.True);

            // relay: rho.me
            relayConf = listenerConf.Relays[1];
            Assert.That(relayConf.ExternalHostName, Is.EqualTo("rho.me"));
            Assert.That(relayConf.DomesticHostName, Is.EqualTo("localhost"));
            Assert.That(relayConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:53808"));
            Assert.That(string.IsNullOrEmpty(relayConf.CertificateId), Is.True);

            // listener: std-ssl
            listenerConf = conf.Listeners["std-ssl"];
            Assert.That(listenerConf.Id, Is.EqualTo("std-ssl"));
            Assert.That(listenerConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:443"));
            Assert.That(listenerConf.IsHttps, Is.True);

            Assert.That(listenerConf.Relays, Is.Not.Null);
            Assert.That(listenerConf.Relays, Has.Count.EqualTo(2));

            // relay: www.rho.me
            relayConf = listenerConf.Relays[0];
            Assert.That(relayConf.ExternalHostName, Is.EqualTo("www.rho.me"));
            Assert.That(relayConf.DomesticHostName, Is.EqualTo("www.rho.me"));
            Assert.That(relayConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:80"));
            Assert.That(relayConf.CertificateId, Is.EqualTo("rho"));

            // relay: rho.me
            relayConf = listenerConf.Relays[1];
            Assert.That(relayConf.ExternalHostName, Is.EqualTo("rho.me"));
            Assert.That(relayConf.DomesticHostName, Is.EqualTo("rho.me"));
            Assert.That(relayConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:80"));
            Assert.That(relayConf.CertificateId, Is.EqualTo("rho"));
        }

        [Test]
        public void CloneConfiguration_ValidOriginalConfiguration_ClonesCorrectly()
        {
            // Arrange
            var proxySection = this.GetProxySection();
            var conf = proxySection.ToProxyPlainConf();

            // Act
            conf = conf.Clone();

            // Assert
            Assert.That(conf, Is.Not.Null);

            var certificates = conf.Certificates;
            Assert.That(certificates, Is.Not.Null);
            Assert.That(certificates, Has.Count.EqualTo(1));

            var certificate = certificates["rho"];
            Assert.That(certificate.Id, Is.EqualTo("rho"));
            Assert.That(certificate.FilePath, Is.EqualTo(@"c:\certs\rho.me.pfx"));
            Assert.That(certificate.Password, Is.EqualTo("doresaq1488"));
            CollectionAssert.AreEquivalent(new[] { "rho.me", }, certificate.Domains.ToArray());

            Assert.That(conf.Listeners, Is.Not.Null);
            Assert.That(conf.Listeners, Has.Count.EqualTo(2));

            // listener: sample
            var listenerConf = conf.Listeners["sample"];
            Assert.That(listenerConf.Id, Is.EqualTo("sample"));
            Assert.That(listenerConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:81"));
            Assert.That(listenerConf.IsHttps, Is.False);
            Assert.That(listenerConf.Relays, Is.Not.Null);
            Assert.That(listenerConf.Relays, Has.Count.EqualTo(2));

            // relay: www.rho.me
            var relayConf = listenerConf.Relays[0];
            Assert.That(relayConf.ExternalHostName, Is.EqualTo("www.rho.me"));
            Assert.That(relayConf.DomesticHostName, Is.EqualTo("localhost"));
            Assert.That(relayConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:53808"));
            Assert.That(string.IsNullOrEmpty(relayConf.CertificateId), Is.True);

            // relay: rho.me
            relayConf = listenerConf.Relays[1];
            Assert.That(relayConf.ExternalHostName, Is.EqualTo("rho.me"));
            Assert.That(relayConf.DomesticHostName, Is.EqualTo("localhost"));
            Assert.That(relayConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:53808"));
            Assert.That(string.IsNullOrEmpty(relayConf.CertificateId), Is.True);

            // listener: std-ssl
            listenerConf = conf.Listeners["std-ssl"];
            Assert.That(listenerConf.Id, Is.EqualTo("std-ssl"));
            Assert.That(listenerConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:443"));
            Assert.That(listenerConf.IsHttps, Is.True);

            Assert.That(listenerConf.Relays, Is.Not.Null);
            Assert.That(listenerConf.Relays, Has.Count.EqualTo(2));

            // relay: www.rho.me
            relayConf = listenerConf.Relays[0];
            Assert.That(relayConf.ExternalHostName, Is.EqualTo("www.rho.me"));
            Assert.That(relayConf.DomesticHostName, Is.EqualTo("www.rho.me"));
            Assert.That(relayConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:80"));
            Assert.That(relayConf.CertificateId, Is.EqualTo("rho"));

            // relay: rho.me
            relayConf = listenerConf.Relays[1];
            Assert.That(relayConf.ExternalHostName, Is.EqualTo("rho.me"));
            Assert.That(relayConf.DomesticHostName, Is.EqualTo("rho.me"));
            Assert.That(relayConf.EndPoint.ToString(), Is.EqualTo("127.0.0.1:80"));
            Assert.That(relayConf.CertificateId, Is.EqualTo("rho"));
        }

        private ProxySection GetProxySection()
        {
            var confText = this.GetType().Assembly.GetResourceText(@"Keeker.Core.Test.Resources.SampleConfig.xml");
            File.WriteAllText(_confFileName, confText, Encoding.UTF8);

            var fileMap = new ConfigurationFileMap(_confFileName);
            var configuration = ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
            var proxySection = (ProxySection)configuration.GetSection("proxy");

            return proxySection;
        }
    }
}

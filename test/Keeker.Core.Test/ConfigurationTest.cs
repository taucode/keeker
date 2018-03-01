using Keeker.Core.Conf;
using NUnit.Framework;
using System.Configuration;
using System.IO;
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
            Assert.That(conf.Listeners, Is.Not.Null);
            Assert.That(conf.Listeners, Has.Count.EqualTo(2));

            // listener: sample
            var listenerConf = conf.Listeners["sample"];
            Assert.That(listenerConf.Id, Is.EqualTo("sample"));
            Assert.That(listenerConf.GetEndPoint().ToString(), Is.EqualTo("192.168.0.12:81"));
            Assert.That(listenerConf.IsHttps, Is.False);

            Assert.That(listenerConf.Hosts, Is.Not.Null);
            Assert.That(listenerConf.Hosts, Has.Count.EqualTo(2));

            // host: www.rho.me
            var hostConf = listenerConf.Hosts["www.rho.me"];
            Assert.That(hostConf.ExternalHostName, Is.EqualTo("www.rho.me"));
            Assert.That(hostConf.HttpRedirect, Is.Not.Null);
            Assert.That(hostConf.HttpRedirect.ToHostName, Is.EqualTo("rho.me"));
            Assert.That(hostConf.Relay, Is.Null);
            Assert.That(hostConf.Certificate, Is.Null);

            // host: rho.me
            hostConf = listenerConf.Hosts["rho.me"];
            Assert.That(hostConf.ExternalHostName, Is.EqualTo("rho.me"));
            Assert.That(hostConf.HttpRedirect, Is.Null);
            var relay = hostConf.Relay;
            Assert.That(relay, Is.Not.Null);
            Assert.That(relay.DomesticHostName, Is.EqualTo("localhost"));
            Assert.That(relay.GetEndPoint().ToString(), Is.EqualTo("127.0.0.1:53808"));
            Assert.That(hostConf.Certificate, Is.Null);

            // listener: std-ssl
            listenerConf = conf.Listeners["std-ssl"];
            Assert.That(listenerConf.Id, Is.EqualTo("std-ssl"));
            Assert.That(listenerConf.GetEndPoint().ToString(), Is.EqualTo("127.0.0.1:443"));
            Assert.That(listenerConf.IsHttps, Is.True);

            Assert.That(listenerConf.Hosts, Is.Not.Null);
            Assert.That(listenerConf.Hosts, Has.Count.EqualTo(2));

            // host: www.rho.me
            hostConf = listenerConf.Hosts["www.rho.me"];
            Assert.That(hostConf.ExternalHostName, Is.EqualTo("www.rho.me"));
            Assert.That(hostConf.HttpRedirect, Is.Not.Null);
            Assert.That(hostConf.HttpRedirect.ToHostName, Is.EqualTo("rho.me"));
            Assert.That(hostConf.Relay, Is.Null);

            var cert = hostConf.Certificate;
            Assert.That(cert, Is.Not.Null);
            Assert.That(cert.FilePath, Is.EqualTo(@"c:\certs\rho.me.pfx"));
            Assert.That(cert.Password, Is.EqualTo(@"doresaq1488"));

            // host: rho.me
            hostConf = listenerConf.Hosts["rho.me"];
            Assert.That(hostConf.ExternalHostName, Is.EqualTo("rho.me"));
            Assert.That(hostConf.HttpRedirect, Is.Null);
            relay = hostConf.Relay;
            Assert.That(relay, Is.Not.Null);
            Assert.That(relay.DomesticHostName, Is.EqualTo("localhost"));
            Assert.That(relay.GetEndPoint().ToString(), Is.EqualTo("127.0.0.1:53808"));

            cert = hostConf.Certificate;
            Assert.That(cert, Is.Not.Null);
            Assert.That(cert.FilePath, Is.EqualTo(@"c:\certs\rho.me.pfx"));
            Assert.That(cert.Password, Is.EqualTo(@"doresaq1488"));
        }

        [Test]
        public void CloneConfiguration_ValidOriginalConfiguration_ClonsesCorrectly()
        {
            // Arrange
            var proxySection = this.GetProxySection();
            var conf = proxySection.ToProxyPlainConf();

            // Act
            conf = conf.Clone();

            // Assert
            Assert.That(conf, Is.Not.Null);
            Assert.That(conf.Listeners, Is.Not.Null);
            Assert.That(conf.Listeners, Has.Count.EqualTo(2));

            // listener: sample
            var listenerConf = conf.Listeners["sample"];
            Assert.That(listenerConf.Id, Is.EqualTo("sample"));
            Assert.That(listenerConf.GetEndPoint().ToString(), Is.EqualTo("192.168.0.12:81"));
            Assert.That(listenerConf.IsHttps, Is.False);

            Assert.That(listenerConf.Hosts, Is.Not.Null);
            Assert.That(listenerConf.Hosts, Has.Count.EqualTo(2));

            // host: www.rho.me
            var hostConf = listenerConf.Hosts["www.rho.me"];
            Assert.That(hostConf.ExternalHostName, Is.EqualTo("www.rho.me"));
            Assert.That(hostConf.HttpRedirect, Is.Not.Null);
            Assert.That(hostConf.HttpRedirect.ToHostName, Is.EqualTo("rho.me"));
            Assert.That(hostConf.Relay, Is.Null);
            Assert.That(hostConf.Certificate, Is.Null);

            // host: rho.me
            hostConf = listenerConf.Hosts["rho.me"];
            Assert.That(hostConf.ExternalHostName, Is.EqualTo("rho.me"));
            Assert.That(hostConf.HttpRedirect, Is.Null);
            var relay = hostConf.Relay;
            Assert.That(relay, Is.Not.Null);
            Assert.That(relay.DomesticHostName, Is.EqualTo("localhost"));
            Assert.That(relay.GetEndPoint().ToString(), Is.EqualTo("127.0.0.1:53808"));
            Assert.That(hostConf.Certificate, Is.Null);

            // listener: std-ssl
            listenerConf = conf.Listeners["std-ssl"];
            Assert.That(listenerConf.Id, Is.EqualTo("std-ssl"));
            Assert.That(listenerConf.GetEndPoint().ToString(), Is.EqualTo("127.0.0.1:443"));
            Assert.That(listenerConf.IsHttps, Is.True);

            Assert.That(listenerConf.Hosts, Is.Not.Null);
            Assert.That(listenerConf.Hosts, Has.Count.EqualTo(2));

            // host: www.rho.me
            hostConf = listenerConf.Hosts["www.rho.me"];
            Assert.That(hostConf.ExternalHostName, Is.EqualTo("www.rho.me"));
            Assert.That(hostConf.HttpRedirect, Is.Not.Null);
            Assert.That(hostConf.HttpRedirect.ToHostName, Is.EqualTo("rho.me"));
            Assert.That(hostConf.Relay, Is.Null);

            var cert = hostConf.Certificate;
            Assert.That(cert, Is.Not.Null);
            Assert.That(cert.FilePath, Is.EqualTo(@"c:\certs\rho.me.pfx"));
            Assert.That(cert.Password, Is.EqualTo(@"doresaq1488"));

            // host: rho.me
            hostConf = listenerConf.Hosts["rho.me"];
            Assert.That(hostConf.ExternalHostName, Is.EqualTo("rho.me"));
            Assert.That(hostConf.HttpRedirect, Is.Null);
            relay = hostConf.Relay;
            Assert.That(relay, Is.Not.Null);
            Assert.That(relay.DomesticHostName, Is.EqualTo("localhost"));
            Assert.That(relay.GetEndPoint().ToString(), Is.EqualTo("127.0.0.1:53808"));

            cert = hostConf.Certificate;
            Assert.That(cert, Is.Not.Null);
            Assert.That(cert.FilePath, Is.EqualTo(@"c:\certs\rho.me.pfx"));
            Assert.That(cert.Password, Is.EqualTo(@"doresaq1488"));
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

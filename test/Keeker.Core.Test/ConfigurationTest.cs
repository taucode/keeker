﻿using NUnit.Framework;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TauCode.Utils.Extensions;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class ConfigurationTest
    {
        [Test]
        public void Configuration_IsValid_ReturnsExpectedResult()
        {
            // Arrange
            var confText = this.GetType().Assembly.GetResourceText(@"Keeker.Core.Test.Resources.SampleConfig.xml");
            var path = Path.GetTempFileName();
            File.WriteAllText(path, confText, Encoding.UTF8);

            var fileMap = new ConfigurationFileMap(path);
            var configuration = ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
            var proxySection = (ProxySection)configuration.GetSection("proxy");

            // Act
            var plainSection = proxySection.ToPlainKeekerSection();

            // Assert
            Assert.That(plainSection, Is.Not.Null);
            Assert.That(plainSection.IpAddress, Is.EqualTo(IPAddress.Parse("127.0.0.1")));
            Assert.That(plainSection.Port, Is.EqualTo(443));

            var hosts = plainSection.Hosts;
            Assert.That(plainSection.Hosts, Has.Count.EqualTo(1));
            var host = hosts.Single().Value;

            Assert.That(host.Name, Is.EqualTo("rho.me"));
            var targets = host.Targets;
            Assert.That(targets, Has.Length.EqualTo(2));

            var target = targets[0];
            Assert.That(target.Name, Is.EqualTo("dev"));
            Assert.That(target.IsActive, Is.EqualTo(true));
            Assert.That(target.Host, Is.EqualTo("localhost"));
            Assert.That(target.IpAddress.ToString(), Is.EqualTo("127.0.0.1"));
            Assert.That(target.Port, Is.EqualTo(53808));

            target = targets[1];
            Assert.That(target.Name, Is.EqualTo("prod"));
            Assert.That(target.IsActive, Is.EqualTo(false));
            Assert.That(target.Host, Is.EqualTo("rho.me"));
            Assert.That(target.IpAddress.ToString(), Is.EqualTo("127.0.0.1"));
            Assert.That(target.Port, Is.EqualTo(80));

        }
    }
}

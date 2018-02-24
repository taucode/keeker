using NUnit.Framework;
using System;
using System.Configuration;
using System.IO;
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
            var keekerSection = (KeekerSection)configuration.GetSection("keeker");

            // Act
            var plainSection = keekerSection.ToPlainKeekerSection();

            // Assert
            Assert.That(plainSection, Is.Not.Null);
            Assert.That(plainSection.IpAddress, Is.EqualTo(IPAddress.Parse("127.0.0.1")));
            Assert.That(plainSection.Port, Is.EqualTo(443));

            throw new NotImplementedException();
        }
    }
}

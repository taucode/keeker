using Keeker.Core.Conf;
using NUnit.Framework;
using System.Configuration;
using System.IO;
using System.Text;
using TauCode.Utils.Extensions;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class ProxyTest
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
        public void Constructor_ValidArguments_RunsOk()
        {
            // Arrange
            var confText = this.GetType().Assembly.GetResourceText(@"Keeker.Core.Test.Resources.SampleConfig.xml");
            File.WriteAllText(_confFileName, confText, Encoding.UTF8);

            var fileMap = new ConfigurationFileMap(_confFileName);
            var configuration = ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
            var proxySection = (ProxySection)configuration.GetSection("proxy");
            var conf = proxySection.ToProxyPlainConf();

            // Act
            var proxy = new Proxy(conf);
            
            // Assert
            var clonedConf = proxy.GetConf();
        }
    }
}

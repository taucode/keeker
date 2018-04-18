
using Keeker.Core.Data;
using NUnit.Framework;
using System;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class HttpHeaderTest
    {
        [Test]
        public void Constructor_ValidArguments_CreatesHeader()
        {
            // Arrange

            // Act
            var header = new HttpHeader("Host", "rho.me");

            // Assert
            Assert.That(header.Name, Is.EqualTo("Host"));
            Assert.That(header.Value, Is.EqualTo("rho.me"));
            Assert.That(header.ToString(), Is.EqualTo("Host: rho.me\r\n"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("rho.me")]
        [TestCase("tau.com")]
        public void Constructor_NameIsNull_ThrowsArgumentNullException(string value)
        {
            // Arrange
            string name = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpHeader(name, value));
            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }

        [Test]
        public void Constructor_ValueIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpHeader("Host", null));
            Assert.That(ex.ParamName, Is.EqualTo("value"));
        }
    }
}

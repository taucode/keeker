
using Keeker.Core.Data;
using NUnit.Framework;
using System;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class HttpHeaderTest
    {
        [Test]
        [TestCase("Host", "rho.me")]
        [TestCase("Strange-Empty-Header", "")]
        [TestCase("Strange-Header", "~!@#$%^&*()_+?><,.;\"'")]
        public void Constructor_ValidArguments_CreatesHeader(string name, string value)
        {
            // Arrange

            // Act
            var header = new HttpHeader(name, value);

            // Assert
            Assert.That(header.Name, Is.EqualTo(name));
            Assert.That(header.Value, Is.EqualTo(value));
            Assert.That(header.ToString(), Is.EqualTo($"{name}: {value}\r\n"));
            Assert.That(header.ByteCount, Is.EqualTo($"{name}: {value}\r\n".Length));
        }

        [Test]
        [TestCase(" Host")]
        [TestCase("Host ")]
        [TestCase("Host\r")]
        [TestCase("Host:")]
        [TestCase("Host&")]
        [TestCase("")]
        public void Constructor_BadName_ThrowsArgumentException(string badName)
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new HttpHeader(badName, "good-value"));

            Assert.That(ex.ParamName, Is.EqualTo("name"));
            Assert.That(ex.Message, Does.StartWith($"Bad header name: '{badName}'"));
        }

        [Test]
        [TestCase("rho.me\r")]
        [TestCase("значение")]
        [TestCase("rho.me\x7f")]
        public void Constructor_BadValue_ThrowsArgumentException(string badValue)
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new HttpHeader("Good-Header", badValue));

            Assert.That(ex.ParamName, Is.EqualTo("value"));
            Assert.That(ex.Message, Does.StartWith($"Bad header value: '{badValue}'"));
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

        [Test]
        public void Parse_ValidInput_ParsesCorrectly()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Parse_TwoSequentialCrLfs_ParsesCorrectly()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Parse_TwoNonSequentialCrLfs_ParsesCorrectly()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Parse_BadFormedHeader_Error()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Parse_NoCrLf_ReturnsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
        }

    }
}

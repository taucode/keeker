using Keeker.Core.Data;
using Keeker.Core.Exceptions;
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
        public void Constructor_ValidArguments_RunsOk(string name, string value)
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
        [TestCase("")] // empty header value is allowed by RFC
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
        [TestCase("\xf0\xf0\xf0\xf0\xf0Host: rho.me\r\nxf0\xf0xf0\xf0xf0\xf0", 5, "rho.me")]
        [TestCase("\xf0\xf0\xf0\xf0\xf0Host: \r\nxf0\xf0xf0\xf0xf0\xf0", 5, "")]
        public void Parse_ValidInput_ParsesCorrectly(string inputString, int start, string expectedHeaderValue)
        {
            // Arrange
            var input = inputString.ToAsciiBytes();

            // Act
            var header = HttpHeader.Parse(input, start);

            // Assert
            Assert.That(header, Is.Not.Null);
            Assert.That(header.Name, Is.EqualTo("Host"));
            Assert.That(header.Value, Is.EqualTo(expectedHeaderValue));
        }

        [Test]
        public void Parse_BufferIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => HttpHeader.Parse(null, 5));
            Assert.That(ex.ParamName, Is.EqualTo("buffer"));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(1000)]
        public void Parse_StartIsOutOfRange_ThrowsArgumentOutOfRangeException(int start)
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => HttpHeader.Parse(new byte[10], start));
            Assert.That(ex.ParamName, Is.EqualTo("start"));
        }

        [Test]
        public void Parse_EmptyInput_ReturnsNull()
        {
            // Arrange
            var input = "\xf0\xf0\xf0\xf0\xf0\r\nxf0\xf0xf0\xf0xf0\xf0".ToAsciiBytes();

            // Act
            var header = HttpHeader.Parse(input, 5);

            // Assert
            Assert.That(header, Is.Null);
        }

        [Test]
        public void Parse_TwoSequentialCrLfs_ParsesCorrectly()
        {
            // Arrange
            var input = "\xf0\xf0\xf0\xf0\xf0Host: rho.me\r\n\r\nHost: taucode.com\xf0\xf0\xf0\xf0\xf0\xf0".ToAsciiBytes();

            // Act
            var header = HttpHeader.Parse(input, 5);

            // Assert
            Assert.That(header, Is.Not.Null);
            Assert.That(header.Name, Is.EqualTo("Host"));
            Assert.That(header.Value, Is.EqualTo("rho.me"));
        }

        [Test]
        public void Parse_TwoNonSequentialCrLfs_ParsesCorrectly()
        {
            // Arrange
            var input = "\xf0\xf0\xf0\xf0\xf0Host: rho.me\r\nHost: wat.net\r\nHost: taucode.com\xf0\xf0\xf0\xf0\xf0\xf0".ToAsciiBytes();

            // Act
            var header = HttpHeader.Parse(input, 5);

            // Assert
            Assert.That(header, Is.Not.Null);
            Assert.That(header.Name, Is.EqualTo("Host"));
            Assert.That(header.Value, Is.EqualTo("rho.me"));
        }

        [Test]
        [TestCase("...Host : rho.me\r\n", 3)]
        [TestCase("...Host:rho.me\r\n", 3)]
        [TestCase("...Host rho.me\r\n", 3)]
        [TestCase("...Host\r\n", 3)]
        public void Parse_BadFormedHeader_ThrowsBadHttpDataException(string inputString, int start)
        {
            // Arrange
            var input = inputString.ToAsciiBytes();

            // Act & Assert
            Assert.Throws<BadHttpDataException>(() => HttpHeader.Parse(input, start));
        }

        [Test]
        [TestCase("...Content Length: 1488\r\n", 3)]
        [TestCase("...Content\0x01Length: 1488\r\n", 3)]
        [TestCase("...Content\0x81Length: 1488\r\n", 3)]
        [TestCase("...Content.Length: 1488\r\n", 3)]
        [TestCase("...Мой-заголовок: 1488\r\n", 3)]
        public void Parse_BadHeaderName_ThrowsBadHttpDataException(string inputString, int start)
        {
            // Arrange
            var input = inputString.ToAsciiBytes();

            // Act & Assert
            Assert.Throws<BadHttpDataException>(() => HttpHeader.Parse(input, start));
        }

        [Test]
        public void Parse_NoCrLf_ThrowsBadHttpDataException()
        {
            // Arrange
            var input = "\xf0\xf0\xf0\xf0\xf0Host: rho.me\r\rxf0\xf0xf0\xf0xf0\xf0".ToAsciiBytes();

            // Act & Assert
            var ex = Assert.Throws<BadHttpDataException>(() => HttpHeader.Parse(input, 5));
            Assert.That(ex.Message, Is.EqualTo("Header-delimiting CRLF not found"));
        }
    }
}

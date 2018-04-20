using Keeker.Core.Data;
using Keeker.Core.Exceptions;
using NUnit.Framework;
using System;
using System.Net;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class HttpStatusLineTest
    {
        [Test]
        [TestCase("HTTP/1.1", HttpStatusCode.Accepted, "Accepted")]
        [TestCase("HTTP/1.1", HttpStatusCode.OK, "OK")]
        public void Constructor_3ArgumentsValid_RunsOk(string version, HttpStatusCode code, string reason)
        {
            // Arrange

            // Act
            var line = new HttpStatusLine(version, code, reason);

            // Assert
            Assert.That(line.Version, Is.EqualTo(version));
            Assert.That(line.Code, Is.EqualTo(code));
            Assert.That(line.Reason, Is.EqualTo(reason));
            Assert.That(line.ToString(), Is.EqualTo($"{version} {(int)code} {reason}\r\n"));
            Assert.That(line.ByteCount, Is.EqualTo($"{version} {(int)code} {reason}\r\n".Length));
            Assert.That(line.ToArray(), Is.EquivalentTo($"{version} {(int)code} {reason}\r\n".ToAsciiBytes()));
        }

        [Test]
        public void Constructor_1ArgumentValid_RunsOk()
        {
            // Arrange

            // Act
            var line = new HttpStatusLine(HttpStatusCode.ExpectationFailed);

            // Assert
            Assert.That(line.Version, Is.EqualTo("HTTP/1.1"));
            Assert.That(line.Code, Is.EqualTo(HttpStatusCode.ExpectationFailed));
            Assert.That(line.Reason, Is.EqualTo("Expectation Failed"));
            Assert.That(line.ToString(), Is.EqualTo("HTTP/1.1 417 Expectation Failed\r\n"));
            Assert.That(line.ByteCount, Is.EqualTo("HTTP/1.1 417 Expectation Failed\r\n".Length));
            Assert.That(line.ToArray(), Is.EquivalentTo("HTTP/1.1 417 Expectation Failed\r\n".ToAsciiBytes()));
        }

        [Test]
        public void Constructor_VersionIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new HttpStatusLine(null, HttpStatusCode.Accepted, "-wrong...reason? :("));
            Assert.That(ex.ParamName, Is.EqualTo("version"));
        }

        [Test]
        public void Constructor_VersionIsInvalid_ThrowsBadHttpDataException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new HttpStatusLine("HTTP-1.1", HttpStatusCode.Accepted, "-wrong...reason? :("));
            Assert.That(ex.ParamName, Is.EqualTo("version"));
            Assert.That(ex.Message, Does.StartWith("Invalid HTTP version"));
        }

        [Test]
        public void Constructor_ReasonIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new HttpStatusLine("HTTP/1.1", HttpStatusCode.Accepted, null));
            Assert.That(ex.ParamName, Is.EqualTo("reason"));
        }

        [Test]
        public void Constructor_ReasonIsInvalid_ThrowsBadHttpDataException()
        {
            // Arrange
            
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new HttpStatusLine("HTTP/1.1", HttpStatusCode.Accepted, "-wrong...reason? :("));
            Assert.That(ex.ParamName, Is.EqualTo("reason"));
        }

        [Test]
        public void Constructor_CustomCodeAndReason_RunsOk()
        {
            // Arrange

            // Act
            var line = new HttpStatusLine("HTTP/1.1", (HttpStatusCode)1488, "Special Reason");

            // Assert
            Assert.That(line.Version, Is.EqualTo("HTTP/1.1"));
            Assert.That((int)line.Code, Is.EqualTo(1488));
            Assert.That(line.Reason, Is.EqualTo("Special Reason"));
        }

        [Test]
        public void Constructor_CustomCodeWithoutReason_RunsOkReasonIsUnknown()
        {
            // Arrange

            // Act
            var line = new HttpStatusLine((HttpStatusCode)1488);

            // Assert
            Assert.That(line.Version, Is.EqualTo("HTTP/1.1"));
            Assert.That((int)line.Code, Is.EqualTo(1488));
            Assert.That(line.Reason, Is.EqualTo("Unknown"));
        }

        [Test]
        [TestCase("\x01\x01\x01HTTP/1.1 200 OK\r\n", "HTTP/1.1", HttpStatusCode.OK, "OK")]
        [TestCase("\x01\x01\x01HTTP/14.88 1488 Very Special Case\r\n", "HTTP/14.88", (HttpStatusCode)1488, "Very Special Case")]
        public void Parse_ValidArguments_ParsesCorrectly(string input, string expectedVersion, HttpStatusCode expectedCode, string expectedReason)
        {
            // Arrange
            var buffer = input.ToAsciiBytes();

            // Act
            var line = HttpStatusLine.Parse(buffer, 3);

            // Assert
            Assert.That(line.Version, Is.EqualTo(expectedVersion));
            Assert.That(line.Code, Is.EqualTo(expectedCode));
            Assert.That(line.Reason, Is.EqualTo(expectedReason));

            var expectedString = $"{expectedVersion} {(int)expectedCode} {expectedReason}\r\n";
            Assert.That(line.ToString(), Is.EqualTo(expectedString));
            Assert.That(line.ByteCount, Is.EqualTo(expectedString.Length));
            Assert.That(line.ToArray(), Is.EquivalentTo(expectedString.ToAsciiBytes()));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(100)]
        public void Parse_BufferIsNull_ThrowsArgumentNullException(int start)
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => HttpStatusLine.Parse(null, start));
            Assert.That(ex.ParamName, Is.EqualTo("buffer"));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(1000)]
        public void Parse_StartIsOutOfRange_ThrowsArgumentOutOfRangeException(int start)
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => HttpStatusLine.Parse(new byte[10], start));
            Assert.That(ex.ParamName, Is.EqualTo("start"));
        }

        [Test]
        [TestCase("\x01\x01\x01 HTTP/1.1  200 OK\r\n")]
        [TestCase("\x01\x01\x01HTTP/1.1  200 OK\r\n")]
        [TestCase("\x01\x01\x01HTTP/1.1 200  OK\r\n")]
        [TestCase("\x01\x01\x01HTTP/1.1 200 OK\r")]
        [TestCase("\x01\x01\x01HTTP/1.1 200 OK\n")]
        [TestCase("\x01\x01\x01HTTP/1.1 200 OK")]
        [TestCase("\x01\x01\x01HTTP/1.1 200a OK")]
        [TestCase("\x01\x01\x01HTTP/1.1 200 Reason?\r\n")]
        [TestCase("\x01\x01\x01\r\n")]
        [TestCase("\x01\x01\x01s")]
        public void Parse_BufferIsInvalid_ThrowsBadHttpDataException(string input)
        {
            // Arrange
            var buffer = input.ToAsciiBytes();

            // Act & Assert
            var ex = Assert.Throws<BadHttpDataException>(() => HttpStatusLine.Parse(buffer, 3));
        }
    }
}

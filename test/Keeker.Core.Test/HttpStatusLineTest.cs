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
            throw new BadHttpDataException();
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new HttpStatusLine(null, HttpStatusCode.Accepted, "-wrong...reason? :("));
            Assert.That(ex.ParamName, Is.EqualTo("version"));
        }

        [Test]
        public void Constructor_ReasonIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            throw new NotImplementedException();
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new HttpStatusLine(null, HttpStatusCode.Accepted, "-wrong...reason? :("));
            Assert.That(ex.ParamName, Is.EqualTo("version"));
        }

        [Test]
        public void Constructor_ReasonIsInvalid_ThrowsBadHttpDataException()
        {
            // Arrange
            throw new NotImplementedException();
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new HttpStatusLine(null, HttpStatusCode.Accepted, "-wrong...reason? :("));
            Assert.That(ex.ParamName, Is.EqualTo("version"));
        }

        [Test]
        public void Constructor_CustomCodeAndReason_RunsOk()
        {
            // Arrange
            throw new NotImplementedException();
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new HttpStatusLine(null, HttpStatusCode.Accepted, "-wrong...reason? :("));
            Assert.That(ex.ParamName, Is.EqualTo("version"));
        }

        [Test]
        public void Constructor_CustomCodeWithoutReason_ThrowsBadHttpDataException()
        {
            // Arrange
            throw new BadHttpDataException();
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new HttpStatusLine(null, HttpStatusCode.Accepted, "-wrong...reason? :("));
            Assert.That(ex.ParamName, Is.EqualTo("version"));
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

        // *** parse
        
        // null
        // start -5
        // bad stream: bad version
        // bad stream: bad code
        // bad stream: bad reason
        // bad stream: violation of spaces

    }
}

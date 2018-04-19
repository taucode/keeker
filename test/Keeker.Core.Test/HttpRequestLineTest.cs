using Keeker.Core.Data;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class HttpRequestLineTest
    {
        [Test]
        [TestCase("GET", "/", "HTTP/1.1")]
        [TestCase("POST", "/action", "HTTP/1.0")]
        public void Constructor_3ArgumentsValid_RunsOk(string method, string uri, string version)
        {
            // Arrange

            // Act
            var line = new HttpRequestLine(new HttpMethod(method), uri, version);

            // Assert
            Assert.That(line.Method.ToString(), Is.EqualTo(method));
            Assert.That(line.RequestUri, Is.EqualTo(uri));
            Assert.That(line.Version, Is.EqualTo(version));
            Assert.That(line.ToString(), Is.EqualTo($"{method} {uri} {version}\r\n"));
            Assert.That(line.ByteCount, Is.EqualTo($"{method} {uri} {version}\r\n".Length));
        }

        [Test]
        [TestCase("GET", "/")]
        [TestCase("POST", "/action")]
        public void Constructor_2ArgumentsValid_RunsOk(string method, string uri)
        {
            // Arrange
            var version = "HTTP/1.1";

            // Act
            var line = new HttpRequestLine(new HttpMethod(method), uri);

            // Assert
            Assert.That(line.Method.ToString(), Is.EqualTo(method));
            Assert.That(line.RequestUri, Is.EqualTo(uri));
            Assert.That(line.Version, Is.EqualTo(version));
            Assert.That(line.ToString(), Is.EqualTo($"{method} {uri} {version}\r\n"));
            Assert.That(line.ByteCount, Is.EqualTo($"{method} {uri} {version}\r\n".Length));
        }

        [Test]
        [TestCase("/good-uri", "HTTP/1.1")]
        [TestCase("/good-uri", "-BAD-VERSION-")]
        [TestCase("/good-uri", null)]
        [TestCase("\rbad-uri", "HTTP/1.1")]
        [TestCase(null, "-BAD-VERSION-")]
        public void Constructor_MethodIsNull_ThrowsArgumentNullException(string uri, string version)
        {
            // Arrange

            // Act & Assert
            var ex1 = Assert.Throws<ArgumentNullException>(() => new HttpRequestLine(null, uri));
            var ex2 = Assert.Throws<ArgumentNullException>(() => new HttpRequestLine(null, uri, version));

            Assert.That(ex1.ParamName, Is.EqualTo("method"));
            Assert.That(ex2.ParamName, Is.EqualTo("method"));
        }

        [Test]
        [TestCase("get", false)]
        [TestCase(" GET", true)]
        [TestCase("GET ", true)]
        [TestCase("\rGET", true)]
        [TestCase("GET1", false)]
        public void Constructor_MethodIsInvalid_ThrowsArgumentNullException(string method, bool expectedFormatException)
        {
            // Arrange
            var badUri = "\rbad-uri";
            var badVersion = "-bad-version-";

            // Act & Assert
            if (expectedFormatException)
            {
                Assert.Throws<FormatException>(() => new HttpRequestLine(new HttpMethod(method), badUri, badVersion));
                Assert.Throws<FormatException>(() => new HttpRequestLine(new HttpMethod(method), badUri));
            }
            else
            {
                var ex1 = Assert.Throws<ArgumentException>(() => new HttpRequestLine(new HttpMethod(method), badUri, badVersion));
                var ex2 = Assert.Throws<ArgumentException>(() => new HttpRequestLine(new HttpMethod(method), badUri));

                Assert.That(ex1.ParamName, Is.EqualTo("method"));
                Assert.That(ex2.ParamName, Is.EqualTo("method"));
            }
        }

        [Test]
        [TestCase("HTTP/1.1")]
        [TestCase("-BAD-VERSION-")]
        [TestCase(null)]
        public void Constructor_UriIsNull_ThrowsArgumentNullException(string version)
        {
            // Arrange

            // Act & Assert
            var ex1 = Assert.Throws<ArgumentNullException>(() => new HttpRequestLine(HttpMethod.Get, null, version));
            var ex2 = Assert.Throws<ArgumentNullException>(() => new HttpRequestLine(HttpMethod.Get, null));

            Assert.That(ex1.ParamName, Is.EqualTo("requestUri"));
            Assert.That(ex2.ParamName, Is.EqualTo("requestUri"));
        }

        [Test]
        [TestCase(" ", "HTTP/1.1")]
        [TestCase("/ ", null)]
        [TestCase(" /", "HTTP/1.1")]
        [TestCase("", "-BAD-VERSION-")]
        [TestCase("/плохой урл", null)]
        public void Constructor_UriIsInvalid_ThrowsArgumentNullException(string uri, string version)
        {
            // Arrange

            // Act & Assert
            var ex1 = Assert.Throws<ArgumentException>(() => new HttpRequestLine(HttpMethod.Get, uri, version));
            var ex2 = Assert.Throws<ArgumentException>(() => new HttpRequestLine(HttpMethod.Get, uri));
            Assert.That(ex1.ParamName, Is.EqualTo("requestUri"));
            Assert.That(ex2.ParamName, Is.EqualTo("requestUri"));
        }

        [Test]
        public void Constructor_VersionIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpRequestLine(HttpMethod.Get, "/", null));
            Assert.That(ex.ParamName, Is.EqualTo("version"));
        }

        [Test]
        [TestCase(" HTTP/1.1")]
        [TestCase("HTTP/1.1 ")]
        [TestCase(" ")]
        [TestCase("")]
        [TestCase("http/1.1")]
        [TestCase("HTTP-1.1")]
        public void Constructor_VersionIsInvalid_ThrowsArgumentNullException(string version)
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new HttpRequestLine(HttpMethod.Get, "/", version));
            Assert.That(ex.ParamName, Is.EqualTo("version"));
        }

        [Test]
        [TestCase("\x01\x01\x01GET /index.html HTTP/1.1\r\n", "GET", "/index.html", "HTTP/1.1")]
        [TestCase("\x01\x01\x01POST /do-action HTTP/1.1\r\n", "POST", "/do-action", "HTTP/1.1")]
        [TestCase("\x01\x01\x01PUT /do-action HTTP/1.0\r\n\r\n", "PUT", "/do-action", "HTTP/1.0")]
        public void Parse_ValidArguments_CreatesLine(string input, string expectedMethod, string expectedUri, string expectedVersion)
        {
            // Arrange
            var buffer = input.ToAsciiBytes();

            // Act
            var line = HttpRequestLine.Parse(buffer, 3);

            // Assert
            Assert.That(line, Is.Not.Null);
            Assert.That(line.Method.ToString(), Is.EqualTo(expectedMethod));
            Assert.That(line.RequestUri, Is.EqualTo(expectedUri));
            Assert.That(line.Version, Is.EqualTo(expectedVersion));
        }
    }
}

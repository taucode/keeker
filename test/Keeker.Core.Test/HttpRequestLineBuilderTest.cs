using Keeker.Core.Data.Builders;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class HttpRequestLineBuilderTest
    {
        // TODO: valid uri chars below
        // ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~:/?#[]@!$&'()*+,;=`%
        [Test]
        public void Constructor_NoParameters_CreatesValidRequestLine()
        {
            // Arrange
            var requestLineBuilder = new HttpRequestLineBuilder();

            // Act
            var line = requestLineBuilder.Build();
            var lineString = line.ToString();

            // Assert
            Assert.That(requestLineBuilder.Method.ToString(), Is.EqualTo("GET"));
            Assert.That(requestLineBuilder.RequestUri, Is.EqualTo("/"));
            Assert.That(requestLineBuilder.Version, Is.EqualTo("HTTP/1.1"));
            Assert.That(lineString, Is.EqualTo("GET / HTTP/1.1\r\n"));
        }

        [Test]
        [TestCase("get")]
        [TestCase(" GET")]
        [TestCase("GET ")]
        [TestCase("")]
        public void Constructor_BadMethod_ThrowsArgumentException(string badMethod)
        {
            // Arrange

            // Act & Assert

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        [TestCase(" /")]
        [TestCase("/ ")]
        [TestCase("")]
        [TestCase("/документ")]
        [TestCase("\"")]
        [TestCase("\x7f")]
        [TestCase("\x1f")]
        [TestCase("\x01")]
        [TestCase("\x81")]
        public void Constructor_BadUri_ThrowsArgumentException(string badUri)
        {
            throw new NotImplementedException(); // bad method? bad uri? bad version?
        }

        [Test]
        [TestCase("HTTP-3")]
        [TestCase("")]
        [TestCase("HTTP/1.1 ")]
        [TestCase("XTTП")]
        [TestCase("HTTP\\1")]
        public void Constructor_BadVersion_ThrowsArgumentException(string badVersion)
        {
            throw new NotImplementedException(); // bad method? bad uri? bad version?
        }

        [Test]
        public void Constructor_ValidUri_CreatesValidRequestLine()
        {
            // Arrange
            var requestLineBuilder = new HttpRequestLineBuilder("/index.html");

            // Act
            var line = requestLineBuilder.Build();
            var lineString = line.ToString();

            // Assert
            Assert.That(requestLineBuilder.Method.ToString(), Is.EqualTo("GET"));
            Assert.That(requestLineBuilder.RequestUri, Is.EqualTo("/index.html"));
            Assert.That(requestLineBuilder.Version, Is.EqualTo("HTTP/1.1"));
            Assert.That(lineString, Is.EqualTo("GET /index.html HTTP/1.1\r\n"));
        }

        [Test]
        public void Constructor_NullUri_ThrowsArgumentNullException()
        {
            // Arrange
            string uri = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpRequestLineBuilder(uri));
            Assert.That(ex.ParamName, Is.EqualTo("uri"));
        }

        [Test]
        public void Constructor_ValidMethodAndUri_CreatesValidRequestLine()
        {
            // Arrange
            var requestLineBuilder = new HttpRequestLineBuilder(HttpMethod.Put, "/do-put-data");

            // Act
            var line = requestLineBuilder.Build();
            var lineString = line.ToString();

            // Assert
            Assert.That(requestLineBuilder.Method.ToString(), Is.EqualTo("PUT"));
            Assert.That(requestLineBuilder.RequestUri, Is.EqualTo("/do-put-data"));
            Assert.That(requestLineBuilder.Version, Is.EqualTo("HTTP/1.1"));
            Assert.That(lineString, Is.EqualTo("PUT /do-put-data HTTP/1.1\r\n"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("/index.html")]
        public void Constructor_NullMethod_ThrowsArgumentNullException(string uri)
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpRequestLineBuilder(null, uri));
            Assert.That(ex.ParamName, Is.EqualTo("method"));
        }

        [Test]
        public void Constructor_ValidMethodAndNullUri_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpRequestLineBuilder(HttpMethod.Delete, null));
            Assert.That(ex.ParamName, Is.EqualTo("uri"));
        }

    }
}

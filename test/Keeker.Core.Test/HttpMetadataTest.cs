using Keeker.Core.Data.Builders;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class HttpMetadataTest
    {
        [Test]
        public void HttpRequestLineBuilder_ConstructorWithNoParameters_CreatesValidRequestLine()
        {
            // Arrange
            var requestLineBuilder = new HttpRequestLineBuilder();

            // Act
            var line = requestLineBuilder.Build();
            var lineString = line.ToString();

            // Assert
            Assert.That(lineString, Is.EqualTo("GET / HTTP/1.1\r\n"));
        }

        [Test]
        public void HttpRequestLineBuilder_ConstructorWithValidUri_CreatesValidRequestLine()
        {
            // Arrange
            var requestLineBuilder = new HttpRequestLineBuilder("/index.html");

            // Act
            var line = requestLineBuilder.Build();
            var lineString = line.ToString();

            // Assert
            Assert.That(lineString, Is.EqualTo("GET /index.html HTTP/1.1\r\n"));
        }

        [Test]
        public void HttpRequestLineBuilder_ConstructorWithNullUri_ThrowsArgumentNullException()
        {   
            // Arrange
            string uri = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpRequestLineBuilder(uri));
            Assert.That(ex.ParamName, Is.EqualTo("uri"));
        }

        [Test]
        public void HttpRequestLineBuilder_ConstructorWithValidMethodAndUri_CreatesValidRequestLine()
        {
            // Arrange
            var requestLineBuilder = new HttpRequestLineBuilder(HttpMethod.Put, "/index.html");

            // Act
            var line = requestLineBuilder.Build();
            var lineString = line.ToString();

            // Assert
            Assert.That(lineString, Is.EqualTo("PUT /index.html HTTP/1.1\r\n"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("/index.html")]
        public void HttpRequestLineBuilder_ConstructorWithNullMethod_ThrowsArgumentNullException(string uri)
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpRequestLineBuilder(null, uri));
            Assert.That(ex.ParamName, Is.EqualTo("method"));
        }

        [Test]
        public void HttpRequestLineBuilder_ConstructorWithValidMethodAndNullUri_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpRequestLineBuilder(HttpMethod.Delete, null));
            Assert.That(ex.ParamName, Is.EqualTo("uri"));
        }

    }
}

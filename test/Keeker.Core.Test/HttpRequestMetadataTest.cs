using Keeker.Core.Data;
using Keeker.Core.Exceptions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class HttpRequestMetadataTest
    {
        [Test]
        public void Constructor_ValidArguments_RunsOk()
        {
            // Arrange
            var line = new HttpRequestLine(HttpMethod.Get, "/");
            var headersArray = new[]
            {
                new HttpHeader("Accept", "text/html, application/xhtml+xml, image/jxr, */*"),
                new HttpHeader("Accept-Language", "ru-RU"),
                new HttpHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299"),
                new HttpHeader("Accept-Encoding", "gzip, deflate"),
                new HttpHeader("Host", "allitebooks.com"),
                new HttpHeader("Connection", "Keep-Alive"),
            };
            var headers = new HttpHeaderCollection(headersArray);

            // Act
            var requestMetadata = new HttpRequestMetadata(line, headers);

            // Assert
            Assert.That(requestMetadata.Line.Method.ToString(), Is.EqualTo("GET"));
            Assert.That(requestMetadata.Line.RequestUri, Is.EqualTo("/"));
            Assert.That(requestMetadata.Line.Version, Is.EqualTo("HTTP/1.1"));

            Assert.That(requestMetadata.Headers.Count(), Is.EqualTo(6));
            Assert.That(requestMetadata.Headers.ElementAt(0), Is.SameAs(headersArray[0]));
            Assert.That(requestMetadata.Headers.ElementAt(1), Is.SameAs(headersArray[1]));
            Assert.That(requestMetadata.Headers.ElementAt(2), Is.SameAs(headersArray[2]));
            Assert.That(requestMetadata.Headers.ElementAt(3), Is.SameAs(headersArray[3]));
            Assert.That(requestMetadata.Headers.ElementAt(4), Is.SameAs(headersArray[4]));
            Assert.That(requestMetadata.Headers.ElementAt(5), Is.SameAs(headersArray[5]));

            var expectedString = @"GET / HTTP/1.1
Accept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive

";
            var toString = requestMetadata.ToString();
            Assert.That(toString, Is.EqualTo(expectedString));

            var bytes = requestMetadata.Serialize();
            Assert.That(bytes, Is.EquivalentTo(expectedString.ToAsciiBytes()));
        }

        [Test]
        public void Constructor_LineIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            
            // Act & Assert
            var ex1 = Assert.Throws<ArgumentNullException>(() => new HttpRequestMetadata(null, null));
            var ex2 = Assert.Throws<ArgumentNullException>(() => new HttpRequestMetadata(null, new HttpHeaderCollection()));

            Assert.That(ex1.ParamName, Is.EqualTo("line"));
            Assert.That(ex2.ParamName, Is.EqualTo("line"));
        }

        [Test]
        public void Constructor_HeadersIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var line = new HttpRequestLine(HttpMethod.Get, "/");

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpRequestMetadata(line, null));
            Assert.That(ex.ParamName, Is.EqualTo("headers"));
        }

        [Test]
        [TestCase(@"жжжGET / HTTP/1.1
Accept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive

")]
        [TestCase(@"жжжGET / HTTP/1.1
Accept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive

жжжAccept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive

")]
        public void Parse_InputIsValid_ParsesCorrectly(string input)
        {
            // Arrange
            var buffer = input.ToAsciiBytes();

            // Act
            var requestMetadata = HttpRequestMetadata.Parse(buffer, 3);

            // Assert
            Assert.That(requestMetadata.Line.Method.ToString(), Is.EqualTo("GET"));
            Assert.That(requestMetadata.Line.RequestUri, Is.EqualTo("/"));
            Assert.That(requestMetadata.Line.Version, Is.EqualTo("HTTP/1.1"));

            Assert.That(requestMetadata.Headers.Count(), Is.EqualTo(6));
            Assert.That(requestMetadata.Headers.ElementAt(0).Name, Is.EqualTo("Accept"));
            Assert.That(requestMetadata.Headers.ElementAt(0).Value, Is.EqualTo("text/html, application/xhtml+xml, image/jxr, */*"));

            Assert.That(requestMetadata.Headers.ElementAt(1).Name, Is.EqualTo("Accept-Language"));
            Assert.That(requestMetadata.Headers.ElementAt(1).Value, Is.EqualTo("ru-RU"));

            Assert.That(requestMetadata.Headers.ElementAt(2).Name, Is.EqualTo("User-Agent"));
            Assert.That(requestMetadata.Headers.ElementAt(2).Value, Is.EqualTo("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299"));

            Assert.That(requestMetadata.Headers.ElementAt(3).Name, Is.EqualTo("Accept-Encoding"));
            Assert.That(requestMetadata.Headers.ElementAt(3).Value, Is.EqualTo("gzip, deflate"));

            Assert.That(requestMetadata.Headers.ElementAt(4).Name, Is.EqualTo("Host"));
            Assert.That(requestMetadata.Headers.ElementAt(4).Value, Is.EqualTo("allitebooks.com"));

            Assert.That(requestMetadata.Headers.ElementAt(5).Name, Is.EqualTo("Connection"));
            Assert.That(requestMetadata.Headers.ElementAt(5).Value, Is.EqualTo("Keep-Alive"));


            var expectedString = @"GET / HTTP/1.1
Accept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive

";
            var toString = requestMetadata.ToString();
            Assert.That(toString, Is.EqualTo(expectedString));

            var bytes = requestMetadata.Serialize();
            Assert.That(bytes, Is.EquivalentTo(expectedString.ToAsciiBytes()));
        }

        [Test]
        public void Parse_BadLine_ThrowsBadHttpDataException()
        {
            // Arrange
            var stringWithBadLine = @"жжжGET-MOAR / HTTP/1.1
Accept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive

";
            var buffer = stringWithBadLine.ToAsciiBytes();

            // Act & Assert
            Assert.Throws<BadHttpDataException>(() => HttpRequestMetadata.Parse(buffer, 3));
        }

        [Test]
        public void Parse_BadHeaders_ThrowsBadHttpDataException()
        {
            // Arrange
            var stringWithBadHeaders = @"жжжGET / HTTP/1.1
Accept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU" +
"\x01Some-bad-header: The-Value" +
@"User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive

";
            var buffer = stringWithBadHeaders.ToAsciiBytes();

            // Act & Assert
            Assert.Throws<BadHttpDataException>(() => HttpRequestMetadata.Parse(buffer, 3));
        }

        [Test]
        public void Parse_OnlyOneCrLf_ThrowsBadHttpDataException()
        {
            // Arrange
            var input = @"жжжGET / HTTP/1.1
Accept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive
";
            var buffer = input.ToAsciiBytes();

            // Act & Assert
            Assert.Throws<BadHttpDataException>(() => HttpRequestMetadata.Parse(buffer, 3));
        }

        [Test]
        public void Parse_NoCrLfs_ThrowsBadHttpDataException()
        {
            // Arrange
            var input = @"жжжGET / HTTP/1.1
Accept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive";
            var buffer = input.ToAsciiBytes();

            // Act & Assert
            Assert.Throws<BadHttpDataException>(() => HttpRequestMetadata.Parse(buffer, 3));
        }
    }
}

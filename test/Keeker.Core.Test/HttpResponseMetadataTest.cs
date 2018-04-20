using Keeker.Core.Data;
using Keeker.Core.Exceptions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class HttpResponseMetadataTest
    {
        [Test]
        public void Constructor_ValidArguments_RunsOk()
        {
            // Arrange
            var line = new HttpStatusLine(HttpStatusCode.OK);
            var headersArray = new[]
            {
                new HttpHeader("Server", "nginx/1.8.0"),
                new HttpHeader("Date", "Fri, 20 Apr 2018 07:15:11 GMT"),
                new HttpHeader("Content-Type", "text/html; charset=UTF-8"),
                new HttpHeader("Transfer-Encoding", "chunked"),
                new HttpHeader("Connection", "keep-alive"),
                new HttpHeader("Vary", "Accept-Encoding"),
                new HttpHeader("X-Powered-By", "PHP/5.4.45"),
                new HttpHeader("X-Pingback", "http://www.allitebooks.com/xmlrpc.php"),
            };
            var headers = new HttpHeaderCollection(headersArray);

            // Act
            var responseMetadata = new HttpResponseMetadata(line, headers);

            // Assert
            Assert.That(responseMetadata.Line.Version, Is.EqualTo("HTTP/1.1"));
            Assert.That((int)responseMetadata.Line.Code, Is.EqualTo(200));
            Assert.That(responseMetadata.Line.Reason, Is.EqualTo("OK"));

            Assert.That(responseMetadata.Headers.Count(), Is.EqualTo(8));
            Assert.That(responseMetadata.Headers.ElementAt(0), Is.SameAs(headersArray[0]));
            Assert.That(responseMetadata.Headers.ElementAt(1), Is.SameAs(headersArray[1]));
            Assert.That(responseMetadata.Headers.ElementAt(2), Is.SameAs(headersArray[2]));
            Assert.That(responseMetadata.Headers.ElementAt(3), Is.SameAs(headersArray[3]));
            Assert.That(responseMetadata.Headers.ElementAt(4), Is.SameAs(headersArray[4]));
            Assert.That(responseMetadata.Headers.ElementAt(5), Is.SameAs(headersArray[5]));

            var expectedString = @"HTTP/1.1 200 OK
Server: nginx/1.8.0
Date: Fri, 20 Apr 2018 07:15:11 GMT
Content-Type: text/html; charset=UTF-8
Transfer-Encoding: chunked
Connection: keep-alive
Vary: Accept-Encoding
X-Powered-By: PHP/5.4.45
X-Pingback: http://www.allitebooks.com/xmlrpc.php

";
            var toString = responseMetadata.ToString();
            Assert.That(toString, Is.EqualTo(expectedString));

            var bytes = responseMetadata.Serialize();
            Assert.That(bytes, Is.EquivalentTo(expectedString.ToAsciiBytes()));
        }

        [Test]
        public void Constructor_LineIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var ex1 = Assert.Throws<ArgumentNullException>(() => new HttpResponseMetadata(null, null));
            var ex2 = Assert.Throws<ArgumentNullException>(() => new HttpResponseMetadata(null, new HttpHeaderCollection()));

            Assert.That(ex1.ParamName, Is.EqualTo("line"));
            Assert.That(ex2.ParamName, Is.EqualTo("line"));
        }

        [Test]
        public void Constructor_HeadersIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var line = new HttpStatusLine(HttpStatusCode.OK);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpResponseMetadata(line, null));
            Assert.That(ex.ParamName, Is.EqualTo("headers"));
        }

        [Test]
        [TestCase(@"жжжHTTP/1.1 200 OK
Server: nginx/1.8.0
Date: Fri, 20 Apr 2018 07:15:11 GMT
Content-Type: text/html; charset=UTF-8
Transfer-Encoding: chunked
Connection: keep-alive
Vary: Accept-Encoding
X-Powered-By: PHP/5.4.45
X-Pingback: http://www.allitebooks.com/xmlrpc.php

")]
        [TestCase(@"жжжHTTP/1.1 200 OK
Server: nginx/1.8.0
Date: Fri, 20 Apr 2018 07:15:11 GMT
Content-Type: text/html; charset=UTF-8
Transfer-Encoding: chunked
Connection: keep-alive
Vary: Accept-Encoding
X-Powered-By: PHP/5.4.45
X-Pingback: http://www.allitebooks.com/xmlrpc.php

HTTP/1.1 200 OK
Server: nginx/1.8.0
Date: Fri, 20 Apr 2018 07:15:11 GMT
Content-Type: text/html; charset=UTF-8
Transfer-Encoding: chunked
Connection: keep-alive
Vary: Accept-Encoding
X-Powered-By: PHP/5.4.45
X-Pingback: http://www.allitebooks.com/xmlrpc.php

")]
        public void Parse_InputIsValid_ParsesCorrectly(string input)
        {
            // Arrange
            var buffer = input.ToAsciiBytes();

            // Act
            var responseMetadata = HttpResponseMetadata.Parse(buffer, 3);

            // Assert
            Assert.That(responseMetadata.Line.Version, Is.EqualTo("HTTP/1.1"));
            Assert.That((int)responseMetadata.Line.Code, Is.EqualTo(200));
            Assert.That(responseMetadata.Line.Reason, Is.EqualTo("OK"));

            Assert.That(responseMetadata.Headers.Count(), Is.EqualTo(8));

            Assert.That(responseMetadata.Headers.ElementAt(0).Name, Is.EqualTo("Server"));
            Assert.That(responseMetadata.Headers.ElementAt(0).Value, Is.EqualTo("nginx/1.8.0"));

            Assert.That(responseMetadata.Headers.ElementAt(1).Name, Is.EqualTo("Date"));
            Assert.That(responseMetadata.Headers.ElementAt(1).Value, Is.EqualTo("Fri, 20 Apr 2018 07:15:11 GMT"));

            Assert.That(responseMetadata.Headers.ElementAt(2).Name, Is.EqualTo("Content-Type"));
            Assert.That(responseMetadata.Headers.ElementAt(2).Value, Is.EqualTo("text/html; charset=UTF-8"));

            Assert.That(responseMetadata.Headers.ElementAt(3).Name, Is.EqualTo("Transfer-Encoding"));
            Assert.That(responseMetadata.Headers.ElementAt(3).Value, Is.EqualTo("chunked"));

            Assert.That(responseMetadata.Headers.ElementAt(4).Name, Is.EqualTo("Connection"));
            Assert.That(responseMetadata.Headers.ElementAt(4).Value, Is.EqualTo("keep-alive"));

            Assert.That(responseMetadata.Headers.ElementAt(5).Name, Is.EqualTo("Vary"));
            Assert.That(responseMetadata.Headers.ElementAt(5).Value, Is.EqualTo("Accept-Encoding"));

            Assert.That(responseMetadata.Headers.ElementAt(6).Name, Is.EqualTo("X-Powered-By"));
            Assert.That(responseMetadata.Headers.ElementAt(6).Value, Is.EqualTo("PHP/5.4.45"));

            Assert.That(responseMetadata.Headers.ElementAt(7).Name, Is.EqualTo("X-Pingback"));
            Assert.That(responseMetadata.Headers.ElementAt(7).Value, Is.EqualTo("http://www.allitebooks.com/xmlrpc.php"));



            var expectedString = @"HTTP/1.1 200 OK
Server: nginx/1.8.0
Date: Fri, 20 Apr 2018 07:15:11 GMT
Content-Type: text/html; charset=UTF-8
Transfer-Encoding: chunked
Connection: keep-alive
Vary: Accept-Encoding
X-Powered-By: PHP/5.4.45
X-Pingback: http://www.allitebooks.com/xmlrpc.php

";
            var toString = responseMetadata.ToString();
            Assert.That(toString, Is.EqualTo(expectedString));

            var bytes = responseMetadata.Serialize();
            Assert.That(bytes, Is.EquivalentTo(expectedString.ToAsciiBytes()));
        }

        [Test]
        public void Parse_BadLine_ThrowsBadHttpDataException()
        {
            // Arrange
            var stringWithBadLine = @"жжжHTTP-1.1 200 OK
Server: nginx/1.8.0
Date: Fri, 20 Apr 2018 07:15:11 GMT
Content-Type: text/html; charset=UTF-8
Transfer-Encoding: chunked
Connection: keep-alive
Vary: Accept-Encoding
X-Powered-By: PHP/5.4.45
X-Pingback: http://www.allitebooks.com/xmlrpc.php

";
            var buffer = stringWithBadLine.ToAsciiBytes();

            // Act & Assert
            Assert.Throws<BadHttpDataException>(() => HttpResponseMetadata.Parse(buffer, 3));
        }

        [Test]
        public void Parse_BadHeaders_ThrowsBadHttpDataException()
        {
            // Arrange
            var stringWithBadHeaders = @"жжжHTTP/1.1 200 OK
Server: nginx/1.8.0
Date: Fri, 20 Apr 2018 07:15:11 GMT
Content-Type: text/html; charset=UTF-8" +
"\x01Some-bad-header: The-Value\r\n" +
@"Transfer-Encoding: chunked
Connection: keep-alive
Vary: Accept-Encoding
X-Powered-By: PHP/5.4.45
X-Pingback: http://www.allitebooks.com/xmlrpc.php

";
            var buffer = stringWithBadHeaders.ToAsciiBytes();

            // Act & Assert
            Assert.Throws<BadHttpDataException>(() => HttpResponseMetadata.Parse(buffer, 3));
        }

        [Test]
        public void Parse_OnlyOneCrLf_ThrowsBadHttpDataException()
        {
            // Arrange
            var input = @"жжжHTTP/1.1 200 OK
Server: nginx/1.8.0
Date: Fri, 20 Apr 2018 07:15:11 GMT
Content-Type: text/html; charset=UTF-8
Transfer-Encoding: chunked
Connection: keep-alive
Vary: Accept-Encoding
X-Powered-By: PHP/5.4.45
X-Pingback: http://www.allitebooks.com/xmlrpc.php
";
            var buffer = input.ToAsciiBytes();

            // Act & Assert
            Assert.Throws<BadHttpDataException>(() => HttpResponseMetadata.Parse(buffer, 3));
        }

        [Test]
        public void Parse_NoCrLfs_ThrowsBadHttpDataException()
        {
            // Arrange
            var input = @"жжжHTTP/1.1 200 OK
Server: nginx/1.8.0
Date: Fri, 20 Apr 2018 07:15:11 GMT
Content-Type: text/html; charset=UTF-8
Transfer-Encoding: chunked
Connection: keep-alive
Vary: Accept-Encoding
X-Powered-By: PHP/5.4.45
X-Pingback: http://www.allitebooks.com/xmlrpc.php";

            var buffer = input.ToAsciiBytes();

            // Act & Assert
            Assert.Throws<BadHttpDataException>(() => HttpResponseMetadata.Parse(buffer, 3));
        }
    }
}

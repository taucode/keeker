using NUnit.Framework;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class HttpResponseMetadataTest
    {
        [Test]
        public void Constructor_ValidArguments_RunsOk()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Constructor_LineIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Constructor_HeadersIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
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
        [TestCase(@"HTTP/1.1 200 OK
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

            // Act

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Parse_BadLine_ThrowsBadHttpDataException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Parse_BadHeaders_ThrowsBadHttpDataException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Parse_OnlyOneCrLf_ThrowsBadHttpDataException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Parse_NoCrLfs_ThrowsBadHttpDataException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail();
        }
    }
}

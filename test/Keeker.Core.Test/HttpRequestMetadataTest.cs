using NUnit.Framework;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class HttpRequestMetadataTest
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

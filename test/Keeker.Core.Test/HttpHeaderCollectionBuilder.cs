using Keeker.Core.Data;
using NUnit.Framework;
using System;
using System.Linq;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class HttpHeaderCollectionBuilder
    {
        private string _initialHeadersString;
        private HttpHeader[] _initialHeaders;

        [SetUp]
        public void SetUp()
        {
            _initialHeadersString = @"Accept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive

";
            _initialHeaders = new []
            {
                new HttpHeader("Accept", "text/html, application/xhtml+xml, image/jxr, */*"),
                new HttpHeader("Accept-Language", "ru-RU"),
                new HttpHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299"),
                new HttpHeader("Accept-Encoding", "gzip, deflate"),
                new HttpHeader("Host", "allitebooks.com"),
                new HttpHeader("Connection", "Keep-Alive"),
            };
        }

        [Test]
        public void Constructor_NoArguments_CreatesEmptyCollection()
        {
            // Arrange

            // Act
            var headers = new HttpHeaderCollection();

            // Assert
            Assert.That(headers, Is.Empty);

            var headersString = headers.ToString();
            Assert.That(headersString, Is.EqualTo("\r\n"));

            var binary = headers.ToArray();
            Assert.That(binary, Is.EquivalentTo(new byte[] { (byte)'\r', (byte)'\n' }));
        }

        [Test]
        public void Constructor_CollectionOfHeaders_CreatesCollectionWithCopiedHeaders()
        {
            // Arrange

            // Act
            var headers = new HttpHeaderCollection(_initialHeaders);

            // Assert
            Assert.That(headers.Count(), Is.EqualTo(6));

            for (int i = 0; i < 6; i++)
            {
                Assert.That(headers.ElementAt(i), Is.SameAs(_initialHeaders[i]));
            }

            var headersString = headers.ToString();
            Assert.That(headersString, Is.EqualTo(_initialHeadersString));

            var binary = headers.ToArray();
            Assert.That(binary, Is.EquivalentTo(_initialHeadersString.ToAsciiBytes()));
        }

        [Test]
        public void Constructor_HeadersIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => new HttpHeaderCollection(null));
            Assert.That(ex.ParamName, Is.EqualTo("headers"));
        }

        [Test]
        public void Constructor_HeadersContainsNull_ThrowsArgumentException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(
                () => new HttpHeaderCollection(new []
                {
                    new HttpHeader("Accept", "text/html, application/xhtml+xml, image/jxr, */*"),
                    new HttpHeader("Accept-Language", "ru-RU"),
                    new HttpHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299"),
                    new HttpHeader("Accept-Encoding", "gzip, deflate"),
                    new HttpHeader("Host", "allitebooks.com"),
                    null,
                    new HttpHeader("Connection", "Keep-Alive"),
                }));

            Assert.That(ex.ParamName, Is.EqualTo("headers"));
            Assert.That(ex.Message, Does.StartWith("'headers' cannot contain nulls"));
        }

        [Test]
        public void Add_ValidHeader_AddsHeader()
        {
            // Arrange
            var headers = new HttpHeaderCollection(_initialHeaders);

            // Act
            var sameHeader = new HttpHeader(headers.ElementAt(0).Name, headers.ElementAt(0).Value);
            headers.Add(sameHeader);

            // Assert
            Assert.That(headers.Count(), Is.EqualTo(7));
            Assert.That(headers.Last(), Is.SameAs(sameHeader));

            var headersString = headers.ToString();
            Assert.That(headersString, Is.EqualTo(@"Accept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive
Accept: text/html, application/xhtml+xml, image/jxr, */*

"));

            var binary = headers.ToArray();
            Assert.That(binary, Is.EquivalentTo(headersString.ToAsciiBytes()));
        }

        [Test]
        public void Add_HeaderIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var headers = new HttpHeaderCollection(_initialHeaders);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => headers.Add(null));
            Assert.That(ex.ParamName, Is.EqualTo("header"));
        }

        [Test]
        public void Add_HeaderIsRepeating_AddsSameHeader()
        {
            // Arrange
            var headers = new HttpHeaderCollection(_initialHeaders);

            // Act
            var sameHeader = new HttpHeader(headers.ElementAt(0).Name, headers.ElementAt(0).Value);
            headers.Add(sameHeader);

            // Assert
            Assert.That(headers.Count(), Is.EqualTo(7));
            Assert.That(headers.Last(), Is.SameAs(sameHeader));

            var headersString = headers.ToString();
            Assert.That(headersString, Is.EqualTo(@"Accept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive
Accept: text/html, application/xhtml+xml, image/jxr, */*

"));

            var binary = headers.ToArray();
            Assert.That(binary, Is.EquivalentTo(headersString.ToAsciiBytes()));
        }

        [Test]
        public void ContansName_NameIsPresent_ReturnsTrue()
        {
            // Arrange
            var headers = new HttpHeaderCollection(_initialHeaders);

            // Act
            var contains = headers.ContainsName("Host");

            // Assert
            Assert.That(contains, Is.True);
        }

        [Test]
        public void ContansName_NameIsNotPresent_ReturnsFalse()
        {
            // Arrange
            var headers = new HttpHeaderCollection(_initialHeaders);

            // Act
            var contains = headers.ContainsName("Strange-Host");

            // Assert
            Assert.That(contains, Is.False);
        }

        [Test]
        public void ContansName_ArgumentIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var headers = new HttpHeaderCollection(_initialHeaders);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => headers.ContainsName(null));
            Assert.That(ex.ParamName, Is.EqualTo("name"));

        }

        [Test]
        [TestCase(@"жжжAccept: text/html, application/xhtml+xml, image/jxr, */*
Accept-Language: ru-RU
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299
Accept-Encoding: gzip, deflate
Host: allitebooks.com
Connection: Keep-Alive

")]
        [TestCase(@"жжжAccept: text/html, application/xhtml+xml, image/jxr, */*
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
            var headers = HttpHeaderCollection.Parse(buffer, 3);

            // Assert
            Assert.That(headers.Count(), Is.EqualTo(6));

            for (var i = 0; i < 6; i++)
            {
                Assert.That(headers.ElementAt(i).ToString(), Is.EqualTo(_initialHeaders[i].ToString()));
            }

            var headersString = headers.ToString();
            Assert.That(headersString, Is.EqualTo(_initialHeadersString));

            var binary = headers.ToArray();
            Assert.That(binary, Is.EquivalentTo(_initialHeadersString.ToAsciiBytes()));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(10000)]
        public void Parse_BufferIsNull_ThrowsArgumentNullException(int start)
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => HttpHeaderCollection.Parse(null, start));
            Assert.That(ex.ParamName, Is.EqualTo("buffer"));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(10000)]
        public void Parse_StartIsOutOfRange_ThrowsArgumentOutOfRangeException(int start)
        {
            // Arrange
            throw new NotImplementedException();

            // Act & Assert
            //var ex = Assert.Throws<ArgumentNullException>(() => HttpHeaderCollection.Parse(null, start));
            //Assert.That(ex.ParamName, Is.EqualTo("buffer"));
        }

        [Test]
        [TestCase("only one crlf at the end")]
        [TestCase("no crlf")]
        [TestCase("bad name")]
        [TestCase("bad value")]
        public void Parse_InputIsValid_ThrowsBadHttpDataException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail(); // don't forget ToString(), ToArray(), etc!
        }
    }
}

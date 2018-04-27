using Keeker.Core.Streams;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Keeker.Core.Test
{
    [TestFixture]
    public class LinkTest
    {
        [Test]
        public void Write_ValidData_IsRead()
        {
            // Arrange
            var link = new Link();
            var data = "Andy";

            var client = link.Stream1;
            var server = link.Stream2;

            var serverBuffer = new byte[data.Length];

            // Act
            client.WriteAll(data.ToAsciiBytes());
            var bytesReadByServer = server.Read(serverBuffer, 0, serverBuffer.Length);
            var nameReadByServer = serverBuffer.ToAsciiString();

            var greeting = $"Hello, {nameReadByServer}!";
            var greetingBytes = greeting.ToAsciiBytes();

            var clientBuffer = new byte[greeting.Length];
            server.WriteAll(greetingBytes);
            var bytesReadByClient = client.Read(clientBuffer, 0, clientBuffer.Length);
            var greetingObtainedByClient = clientBuffer.ToAsciiString();

            // Assert
            Assert.That(bytesReadByServer, Is.EqualTo(data.Length));
            Assert.That(nameReadByServer, Is.EqualTo("Andy"));

            Assert.That(bytesReadByClient, Is.EqualTo(greeting.Length));
            Assert.That(greetingObtainedByClient, Is.EqualTo("Hello, Andy!"));

            link.Dispose();
        }

        [Test]
        public void Read_WriteZeroBytes_Reads()
        {
            // Arrange
            var link = new Link();

            var client = (LinkStream)link.Stream1;
            var server = (LinkStream)link.Stream2;

            byte[] serverBuffer = new byte[10];
            int byteCountReadByServer = -999; // just to not be 0

            // Act & Assert
            var readingTask = new Task(() =>
            {
                byteCountReadByServer = server.Read(serverBuffer, 0, 10);
            });
            readingTask.Start();

            Thread.Sleep(10);
            Assert.That(server.IsReading, Is.True);

            client.Write(new byte[0], 0, 0);

            Thread.Sleep(10);
            Assert.That(server.IsReading, Is.False);

            Assert.That(byteCountReadByServer, Is.Zero);

            link.Dispose();
        }
    }
}

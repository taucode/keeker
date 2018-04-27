using System;
using System.IO;

namespace Keeker.Core.Streams
{
    public class TransparentByteSender : IByteSender
    {
        public TransparentByteSender(Stream targetStream)
        {
            this.TargetStream = targetStream ?? throw new ArgumentNullException(nameof(targetStream));
        }

        public void Send(byte[] buffer, int offset, int count)
        {
            this.TargetStream.Write(buffer, offset, count);
        }

        public Stream TargetStream { get; }
    }
}

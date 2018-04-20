using System;
using System.IO;

namespace Keeker.Core.Streams
{
    public class PipeStream : Stream
    {
        private readonly ByteAccumulator _to;
        private readonly ByteAccumulator _from;

        public PipeStream(
            ByteAccumulator to,
            ByteAccumulator from)
        {
            _to = to;
            _from = from;
        }

        public override void Flush()
        {
            // idle
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}

using System;
using System.Linq;

namespace Keeker.Core.Streams
{
    public class ByteDelivery
    {
        private readonly byte[] _bytes;
        private int _position;

        public ByteDelivery(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            _bytes = new byte[count];
            Buffer.BlockCopy(buffer, offset, _bytes, 0, count);
        }

        public byte[] GetAllBytes()
        {
            return _bytes.ToArray();
        }

        public int Length => _bytes.Length;

        public int Position => _position;

        public int Remaining => this.Length - this.Position;

        public byte[] Deliver(int count)
        {
            if (count <= 0 || count > Remaining)
            {
                throw new ArgumentOutOfRangeException();
            }

            var portion = new byte[count];
            Buffer.BlockCopy(_bytes, _position, portion, 0, count);
            _position += count;

            return portion;
        }
    }
}

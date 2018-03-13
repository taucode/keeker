using System;

namespace Keeker.Core.TheDevices
{
    public class AutoBuffer
    {
        private byte[] _buffer;

        public AutoBuffer()
        {
            _buffer = new byte[0];
        }

        public void Allocate(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count > _buffer.Length)
            {
                _buffer = new byte[count];
            }
        }

        public byte[] Buffer => _buffer;
    }
}

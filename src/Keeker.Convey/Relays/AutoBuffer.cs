using System;

namespace Keeker.Convey.Relays
{
    public class AutoBuffer
    {
        private byte[] _raw;

        public AutoBuffer()
        {
            _raw = new byte[0];
        }

        public void Allocate(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count > _raw.Length)
            {
                _raw = new byte[count];
            }
        }

        public byte[] Raw => _raw;
    }
}

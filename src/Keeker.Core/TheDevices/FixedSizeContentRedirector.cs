using System;
using System.IO;

namespace Keeker.Core.TheDevices
{
    public class FixedSizeContentRedirector
    {
        private readonly Stream _sourceStream;
        private readonly Stream _destinationStream;
        private readonly AutoBuffer _readBuffer;

        public FixedSizeContentRedirector(
            Stream sourceStream,
            Stream destinationStream,
            AutoBuffer buffer)
        {
            _sourceStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
            _destinationStream = destinationStream ?? throw new ArgumentNullException(nameof(destinationStream));
            _readBuffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        }

        public void Redirect(int length)
        {
            const int CONTENT_TRANSFER_PORTION_SIZE = 10000;

            var remaining = length;
            _readBuffer.Allocate(CONTENT_TRANSFER_PORTION_SIZE);

            while (true)
            {
                if (remaining == 0)
                {
                    break;
                }

                var portionSize = Math.Min(remaining, _readBuffer.Buffer.Length);
                var bytesReadCount = _sourceStream.Read(_readBuffer.Buffer, 0, portionSize);
                _destinationStream.Write(_readBuffer.Buffer, 0, bytesReadCount);

                remaining -= bytesReadCount;
            }
        }
    }
}

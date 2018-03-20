using Keeker.Core.Relays;
using System.IO;

namespace Keeker.Core.Redirectors.DataRedirectors
{
    public class FixedSizeDataRedirector : DataRedirector
    {
        private const int CONTENT_TRANSFER_PORTION_SIZE = 10000;

        private readonly Stream _sourceStream;
        private readonly AutoBuffer _sourceBuffer;
        private readonly Stream _destinationStream;
        private readonly int _length;

        public FixedSizeDataRedirector(
            Stream sourceStream,
            AutoBuffer sourceBuffer,
            Stream destinationStream,
            int length)
        {
            _sourceStream = sourceStream;
            _sourceBuffer = sourceBuffer;
            _destinationStream = destinationStream;
            _length = length;
        }

        public override void Redirect()
        {
            //var remaining = _length;
            _sourceBuffer.Allocate(CONTENT_TRANSFER_PORTION_SIZE);

            CoreHelper.RedirectStream(_sourceStream, _destinationStream, _sourceBuffer.Raw, _length);

            //while (true)
            //{
            //    if (remaining == 0)
            //    {
            //        break;
            //    }

            //    var portionSize = Math.Min(remaining, _sourceBuffer.Buffer.Length);
            //    var bytesReadCount = _sourceStream.Read(_sourceBuffer.Buffer, 0, portionSize);
            //    _destinationStream.Write(_sourceBuffer.Buffer, 0, bytesReadCount);

            //    remaining -= bytesReadCount;
            //}
        }
    }
}

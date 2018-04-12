using Keeker.Core;
using System.IO;
using System.Threading;

namespace Keeker.Convey.Relays.ContentRedirectors
{
    public class FixedSizeContentRedirector : ContentRedirector
    {
        private const int CONTENT_TRANSFER_PORTION_SIZE = 10000;

        private readonly ManualResetEvent _signal;
        private readonly Stream _sourceStream;
        private readonly AutoBuffer _sourceBuffer;
        private readonly Stream _destinationStream;
        private readonly int _length;

        public FixedSizeContentRedirector(
            ManualResetEvent signal,
            Stream sourceStream,
            AutoBuffer sourceBuffer,
            Stream destinationStream,
            int length)
        {
            _signal = signal;
            _sourceStream = sourceStream;
            _sourceBuffer = sourceBuffer;
            _destinationStream = destinationStream;
            _length = length;
        }

        public override void Redirect()
        {
            _sourceBuffer.Allocate(CONTENT_TRANSFER_PORTION_SIZE);

            CoreHelper.RedirectStream(_signal, _sourceStream, _destinationStream, _sourceBuffer.Raw, _length);
        }
    }
}

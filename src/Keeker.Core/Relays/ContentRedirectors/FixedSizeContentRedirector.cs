using System.IO;

namespace Keeker.Core.Relays.ContentRedirectors
{
    public class FixedSizeContentRedirector : ContentRedirector
    {
        private const int CONTENT_TRANSFER_PORTION_SIZE = 10000;

        private readonly Stream _sourceStream;
        private readonly AutoBuffer _sourceBuffer;
        private readonly Stream _destinationStream;
        private readonly int _length;

        public FixedSizeContentRedirector(
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
            _sourceBuffer.Allocate(CONTENT_TRANSFER_PORTION_SIZE);

            CoreHelper.RedirectStream(_sourceStream, _destinationStream, _sourceBuffer.Raw, _length);
        }
    }
}

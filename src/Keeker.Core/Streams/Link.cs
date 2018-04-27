using System;
using System.IO;
using System.Threading;

namespace Keeker.Core.Streams
{
    public class Link : IDisposable
    {
        private readonly ByteAccumulator _byteAccumulator1;
        private readonly ByteAccumulator _byteAccumulator2;

        private readonly LinkStream _linkStream1;
        private readonly AutoResetEvent _toSignal1;

        private readonly LinkStream _linkStream2;
        private readonly AutoResetEvent _toSignal2;

        public Link()
        {
            this.Id = Guid.NewGuid().ToString();

            _byteAccumulator1 = new ByteAccumulator();
            _byteAccumulator2 = new ByteAccumulator();
            _toSignal1 = new AutoResetEvent(false);
            _toSignal2 = new AutoResetEvent(false);

            _linkStream1 = new LinkStream(
                this.Id,
                _byteAccumulator1,
                _toSignal1,
                _byteAccumulator2,
                _toSignal2);

            _linkStream2 = new LinkStream(
                this.Id,
                _byteAccumulator2,
                _toSignal2,
                _byteAccumulator1,
                _toSignal1);
        }

        public string Id { get; }

        public Stream Stream1 => _linkStream1;

        public Stream Stream2 => _linkStream2;

        public void Dispose()
        {
            _linkStream1.Dispose();
            _linkStream2.Dispose();

            _toSignal1.Dispose();
            _toSignal2.Dispose();
        }
    }
}

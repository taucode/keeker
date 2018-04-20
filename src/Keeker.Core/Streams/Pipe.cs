using System;
using System.IO;
using System.Threading;

namespace Keeker.Core.Streams
{
    public class Pipe : IDisposable
    {
        private readonly ByteAccumulator _byteAccumulator1;
        private readonly ByteAccumulator _byteAccumulator2;

        private readonly PipeStream _pipeStream1;
        private readonly AutoResetEvent _toSignal1;

        private readonly PipeStream _pipeStream2;
        private readonly AutoResetEvent _toSignal2;

        public Pipe()
        {
            _byteAccumulator1 = new ByteAccumulator();
            _byteAccumulator2 = new ByteAccumulator();
            _toSignal1 = new AutoResetEvent(false);
            _toSignal2 = new AutoResetEvent(false);

            _pipeStream1 = new PipeStream(
                _byteAccumulator1,
                _toSignal1,
                _byteAccumulator2,
                _toSignal2);

            _pipeStream2 = new PipeStream(
                _byteAccumulator2,
                _toSignal2,
                _byteAccumulator1,
                _toSignal1);
        }

        public Stream Stream1 => _pipeStream1;

        public Stream Stream2 => _pipeStream2;

        public void Dispose()
        {
            _pipeStream1.Dispose();
            _pipeStream2.Dispose();

            _toSignal1.Dispose();
            _toSignal2.Dispose();
        }
    }
}

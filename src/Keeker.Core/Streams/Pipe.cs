using System;
using System.IO;

namespace Keeker.Core.Streams
{
    public class Pipe : IDisposable
    {
        private readonly ByteAccumulator _byteAccumulator1;
        private readonly ByteAccumulator _byteAccumulator2;
        private readonly PipeStream _pipeStream1;
        private readonly PipeStream _pipeStream2;

        public Pipe()
        {
            _byteAccumulator1 = new ByteAccumulator();
            _byteAccumulator2 = new ByteAccumulator();

            _pipeStream1 = new PipeStream(_byteAccumulator1, _byteAccumulator2);
            _pipeStream1 = new PipeStream(_byteAccumulator2, _byteAccumulator1);
        }

        public Stream Stream1 => _pipeStream1;

        public Stream Stream2 => _pipeStream2;

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

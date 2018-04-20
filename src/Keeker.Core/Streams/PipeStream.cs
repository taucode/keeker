using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Keeker.Core.Streams
{
    public class PipeStream : Stream
    {
        private const int WAIT_MILLISECONDS = 1;
        
        private readonly ByteAccumulator _to;
        private readonly AutoResetEvent _toSignal;

        private readonly ByteAccumulator _from;
        private readonly AutoResetEvent _fromSignal;

        private bool _isReading;
        private bool _isWriting;
        private bool _isDisposed;
        private readonly object _lock;

        public PipeStream(
            ByteAccumulator to,
            ByteAccumulator from)
        {
            _to = to;
            _toSignal = new AutoResetEvent(false);

            _from = from;
            _fromSignal = new AutoResetEvent(false);

            _lock = new object();

        }

        public override void Flush()
        {
            // idle
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (_lock)
            {
                if (_isReading)
                {
                    throw new InvalidOperationException("Read operation already in progress");
                }

                _isReading = true;
            }

            try
            {
                while (true)
                {
                    lock (_lock)
                    {
                        if (_isDisposed)
                        {
                            throw new ObjectDisposedException(this.GetType().Name);
                        }
                    }

                    var countBitten = _from.Bite(buffer, offset, count);
                    if (countBitten == 0)
                    {
                        // no bytes right now.
                        var gotSignal = _fromSignal.WaitOne(WAIT_MILLISECONDS);
                        if (gotSignal)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        return countBitten;
                    }
                }
            }
            finally
            {
                lock (_lock)
                {
                    _isReading = false;
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (_lock)
            {


                if (_isWriting)
                {
                    throw new InvalidOperationException("Write operation already in progress");
                }

                _isWriting = true;
            }

            try
            {
                _to.Put(buffer, offset, count);
                _toSignal.Set();
            }
            finally 
            {
                lock (_lock)
                {
                    _isWriting = false;
                }
            }
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            lock (_lock)
            {
                _isDisposed = true;
            }

            this.WaitStopReadingAndWriting();

            _toSignal.Dispose();
            _fromSignal.Dispose();
        }

        private void WaitStopReadingAndWriting()
        {
            // todo7[ak]: ugly!
            var task = new Task(() =>
            {
                while (true)
                {
                    lock (_lock)
                    {
                        var stopped = !_isReading && !_isWriting;
                        if (stopped)
                        {
                            return;
                        }
                    }

                    Thread.Sleep(WAIT_MILLISECONDS);
                }
            });

            task.Start();
            task.Wait();
        }
    }
}

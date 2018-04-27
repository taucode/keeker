using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Keeker.Core.Streams
{
    public class LinkStream : Stream
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

        public LinkStream(
            ByteAccumulator to,
            AutoResetEvent toSingal,
            ByteAccumulator from,
            AutoResetEvent fromSignal)
        {
            _to = to;
            _toSignal = toSingal;

            _from = from;
            _fromSignal = fromSignal;

            _lock = new object();

        }

        public override void Flush()
        {
            lock (_lock)
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().Name);
                }
            }

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
                            var countBittenAfterWaiting = _from.Bite(buffer, offset, count);
                            return countBittenAfterWaiting; // we've got signal, so return wheither we have bitten or not.
                        }
                    }
                    else
                    {
                        // got some bytes
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

                if (_isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                _isWriting = true;
            }

            try
            {
                if (count > 0)
                {
                    _to.Put(buffer, offset, count);
                }

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

        public bool IsReading
        {
            get
            {
                lock (_lock)
                {
                    return _isReading;
                }
            }
        }

        public bool IsWriting
        {
            get
            {
                lock (_lock)
                {
                    return _isWriting;
                }
            }
        }

        public bool IsDisposed
        {
            get
            {
                lock (_lock)
                {
                    return _isDisposed;
                }
            }
        }
    }
}

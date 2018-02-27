using System;
using System.IO;

namespace Keeker.Core
{
    public class KeekStream : Stream
    {
        #region Fields

        private readonly Stream _innerStream;
        private readonly object _lock;
        //private readonly List<byte[]> _peekedSegments;

        private readonly ByteAccumulator _accumulator;

        #endregion

        #region Constructor

        public KeekStream(Stream innerStream)
        {
            _innerStream = innerStream ?? throw new ArgumentException(nameof(innerStream));
            //_peekedSegments = new List<byte[]>();
            _accumulator = new ByteAccumulator();
            _lock = new object();
        }

        #endregion

        #region Overridden from  Stream

        public override void Flush()
        {
            throw new System.NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new System.NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (_lock)
            {
                if (_accumulator.IsEmpty)
                {
                    // read all from inner stream
                    var readFromInnerStremBytesCount = _innerStream.Read(buffer, offset, count);
                    return readFromInnerStremBytesCount;
                }
                else
                {
                    // first, bite from accumulator
                    var bittenBytesCount = _accumulator.Bite(buffer, offset, count);
                    var readFromInnerStreamBytesCount = 0;

                    var remaining = count - bittenBytesCount;

                    // second, read from inner stream if still need to.
                    if (remaining > 0)
                    {
                        readFromInnerStreamBytesCount = _innerStream.Read(buffer, offset + bittenBytesCount, remaining);
                    }

                    var totalRead = bittenBytesCount + readFromInnerStreamBytesCount;
                    return totalRead;
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (_lock)
            {
                _innerStream.Write(buffer, offset, count);
            }
        }

        public override bool CanRead => true;

        public override bool CanSeek
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanWrite => true;

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Public

        public int ReadInnerStream(int maxCount)
        {
            if (maxCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCount));
            }

            lock (_lock)
            {
                var buffer = new byte[maxCount];
                var bytesRead = _innerStream.Read(buffer, 0, maxCount);
                if (bytesRead == 0)
                {
                    return 0; // we haven't read anything from the inner stream
                }
                else
                {
                    _accumulator.Put(buffer, 0, bytesRead);
                    return bytesRead;
                }
            }
        }

        public int Peek(byte[] buffer, int offset, int count)
        {
            return _accumulator.Peek(buffer, offset, count);
        }

        #endregion
    }
}

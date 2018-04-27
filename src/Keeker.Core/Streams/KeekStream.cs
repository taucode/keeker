using System;
using System.IO;
using System.Net.Sockets;

namespace Keeker.Core.Streams
{
    public class KeekStream : Stream
    {
        #region Fields

        private readonly Stream _innerStream;
        private readonly bool _leaveInnerStreamOpen;
        private readonly object _readLock;

        private readonly ByteAccumulator _accumulator;
        private IByteSender _byteSender;

        #endregion

        #region Constructor

        public KeekStream(Stream innerStream, bool leaveInnerStreamOpen)
        {
            _innerStream = innerStream ?? throw new ArgumentException(nameof(innerStream));
            _byteSender = new TransparentByteSender(_innerStream);
            _leaveInnerStreamOpen = leaveInnerStreamOpen;
            _accumulator = new ByteAccumulator();
            _readLock = new object();
        }

        #endregion

        #region Overridden from Stream

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
            lock (_readLock)
            {
                if (_accumulator.IsEmpty)
                {
                    // read all from inner stream
                    var readFromInnerStreamBytesCount = _innerStream.Read(buffer, offset, count);
                    return readFromInnerStreamBytesCount;
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
            _byteSender.Send(buffer, offset, count);
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

        protected override void Dispose(bool disposing)
        {
            if (!_leaveInnerStreamOpen)
            {
                _innerStream.Dispose();
            }

            if (_innerStream is NetworkStream)
            {
                var ns = (NetworkStream)_innerStream; // todo000000[ak] what is going on here?!


                throw new NotImplementedException();

                

                

                

            }
        }

        #endregion

        #region Public

        public int ReadInnerStream(int maxCount, Action<byte[], int> callback = null)
        {
            if (maxCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCount));
            }

            lock (_readLock)
            {
                var buffer = new byte[maxCount];
                var bytesRead = _innerStream.Read(buffer, 0, maxCount);

                //if (bytesRead == 0)
                //{
                //    throw new NotImplementedException(); // tood909090[ak] temp
                //}

                callback?.Invoke(buffer, bytesRead);

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
            lock (_readLock)
            {
                return _accumulator.Peek(buffer, offset, count);
            }
        }

        public int PeekIndexOf(byte[] subarray)
        {
            lock (_readLock)
            {
                var allBytes = _accumulator.PeekAll(); // todo0[ak] not much good
                var index = allBytes.IndexOfSubarray(subarray, 0, allBytes.Length);
                return index;
            }
        }

        public int AccumulatedBytesCount
        {
            get { return _accumulator.Count; }
        }

        public IByteSender ByteSender
        {
            get
            {
                return _byteSender;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(ByteSender));
                }

                if (!ReferenceEquals(this._innerStream, _byteSender.TargetStream))
                {
                    throw new InvalidOperationException("ByteSender's stream is not the same as inner stream of this instance");
                }

                _byteSender = value;
            }
        }

        #endregion
    }
}

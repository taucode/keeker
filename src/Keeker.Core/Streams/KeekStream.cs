using Keeker.Core.ByteSenders;
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
        private readonly bool _isFragmented;
        private readonly IByteSender _byteSender;

        #endregion

        #region Constructor

        public KeekStream(Stream innerStream, bool leaveInnerStreamOpen, bool isFragmented = false)
        {
            _innerStream = innerStream ?? throw new ArgumentException(nameof(innerStream));
            _byteSender = new TransparentByteSender(_innerStream);
            _leaveInnerStreamOpen = leaveInnerStreamOpen;
            _accumulator = new ByteAccumulator();
            _readLock = new object();
            _isFragmented = isFragmented;
            _byteSender = _isFragmented ?
                (IByteSender)new TransparentByteSender(_innerStream) : 
                (IByteSender)new FragmentedByteSender(_innerStream);
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
                    this.ReadFromInnerStream?.Invoke(buffer, offset, readFromInnerStreamBytesCount);

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
                        this.ReadFromInnerStream?.Invoke(buffer, offset + bittenBytesCount, readFromInnerStreamBytesCount);
                    }

                    var totalRead = bittenBytesCount + readFromInnerStreamBytesCount;
                    return totalRead;
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _byteSender.Send(buffer, offset, count);
            if (!_isFragmented)
            {
                this.Written.Invoke(buffer, offset, count);
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

        public int ReadInnerStream(int maxCount)
        {
            if (maxCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCount));
            }

            lock (_readLock)
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
                    this.ReadFromInnerStream?.Invoke(buffer, 0, bytesRead);
                    return bytesRead;
                }
            }
        }

        public void ReleaseWrittenFragment(int byteCount)
        {
            if (_isFragmented)
            {
                var relesedBytes = ((FragmentedByteSender)_byteSender).Release(byteCount);
                this.Written?.Invoke(relesedBytes, 0, byteCount);
            }
            else
            {
                throw new InvalidOperationException("Stream is not fragmented");
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

        public event Action<byte[], int, int> ReadFromInnerStream;

        public event Action<byte[], int, int> Written;

        #endregion
    }
}

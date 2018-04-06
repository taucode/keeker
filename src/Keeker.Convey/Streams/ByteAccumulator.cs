using System;
using System.Collections.Generic;

namespace Keeker.Convey.Streams
{
    public class ByteAccumulator
    {
        private class Segment
        {
            private readonly byte[] _bytes;
            private int _ownOffset;

            internal Segment(byte[] buffer, int offset, int count)
            {
                _bytes = new byte[count];
                Buffer.BlockCopy(buffer, offset, _bytes, 0, count);
                _ownOffset = 0;
            }

            internal int CopyTo(byte[] buffer, int offset, int count)
            {
                var copiedCount = Math.Min(count, this.GetLength());
                Buffer.BlockCopy(_bytes, _ownOffset, buffer, offset, copiedCount);
                return copiedCount;
            }

            internal int Bite(byte[] buffer, int offset, int count)
            {
                var countBitten = Math.Min(count, this.GetLength());
                Buffer.BlockCopy(_bytes, _ownOffset, buffer, offset, countBitten);
                _ownOffset += countBitten;
                return countBitten;
            }

            internal int GetLength()
            {
                return _bytes.Length - _ownOffset;
            }
        }
        
        private readonly Queue<Segment> _segments;
        private readonly object _lock;

        public ByteAccumulator()
        {
            _segments = new Queue<Segment>();
            _lock = new object();
        }

        public void Put(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            lock (_lock)
            {
                var segment = new Segment(buffer, offset, count);
                _segments.Enqueue(segment);
            }
        }

        public int Peek(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count == 0)
            {
                return 0; // no copying took place
            }

            var copiedTotal = 0;
            var remaining = count;
            var current = offset;

            lock (_lock)
            {

                foreach (var segment in _segments)
                {
                    if (remaining == 0)
                    {
                        break;
                    }

                    var copiedBySegment = segment.CopyTo(buffer, current, remaining);

                    copiedTotal += copiedBySegment;
                    remaining -= copiedBySegment;
                    current += copiedBySegment;

                    if (remaining == 0)
                    {
                        break;
                    }
                }

                return copiedTotal;
            }
        }

        public int Bite(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count == 0)
            {
                return 0; // no actual biting
            }

            var totalBitten = 0;
            var current = offset;
            var remaining = count;

            lock (_lock)
            {
                while (true)
                {
                    if (_segments.Count == 0)
                    {
                        break;
                    }

                    if (remaining == 0)
                    {
                        break;
                    }

                    var segment = _segments.Peek();
                    var bittenFromSegment = segment.Bite(buffer, current, remaining);

                    totalBitten += bittenFromSegment;
                    current += bittenFromSegment;
                    remaining -= bittenFromSegment;

                    if (segment.GetLength() == 0)
                    {
                        _segments.Dequeue();
                    }
                }

                return totalBitten;
            }
        }

        public bool IsEmpty
        {
            get
            {
                lock (_lock)
                {
                    return _segments.Count == 0;
                }
            }
        }

        public int Count
        {
            get
            {
                // todo1[ak] optimize/cache?
                var count = 0;
                lock (_lock)
                {
                    foreach (var segment in _segments)
                    {
                        count += segment.GetLength();
                    }
                }

                return count;
            }
        }
    }
}

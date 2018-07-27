using Keeker.Core.Streams;
using System;
using System.Collections.Generic;
using System.IO;

namespace Keeker.Core.ByteSenders
{
    internal class FragmentedByteSender : IByteSender
    {
        private readonly Queue<ByteDelivery> _deliveries;

        internal FragmentedByteSender(Stream targetStream)
        {
            this.TargetStream = targetStream ?? throw new ArgumentNullException(nameof(targetStream));
            _deliveries = new Queue<ByteDelivery>();
        }

        public void Send(byte[] buffer, int offset, int count)
        {
            var delivery = new ByteDelivery(buffer, offset, count);
            _deliveries.Enqueue(delivery);
        }

        public Stream TargetStream { get; }

        public byte[] Release(int count)
        {
            var delivery = _deliveries.Peek();
            var portion = delivery.Deliver(count);
            this.TargetStream.Write(portion, 0, count);

            if (delivery.Remaining == 0)
            {
                _deliveries.Dequeue();
            }

            return portion;
        }
    }
}

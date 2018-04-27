using System;
using System.Collections.Generic;
using System.IO;

namespace Keeker.Core.Streams
{
    public class FragmentedByteSender : IByteSender
    {
        private readonly Queue<ByteDelivery> _deliveries;

        public FragmentedByteSender(Stream targetStream)
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

        public void Release(int count)
        {
            var delivery = _deliveries.Peek();
            var portion = delivery.Deliver(count);
            this.TargetStream.Write(portion, 0, count);
        }
    }
}

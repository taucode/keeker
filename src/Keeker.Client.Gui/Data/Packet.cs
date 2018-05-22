using System;

namespace Keeker.Client.Gui.Data
{
    public class Packet
    {
        public Packet(byte[] bytes, DateTime time)
        {
            this.Bytes = bytes;
            this.Time = time;
        }

        public byte[] Bytes { get; }

        public DateTime Time { get; }
    }
}

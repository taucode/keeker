using Keeker.Core.Relays;
using System;

namespace Keeker.Core.Events
{
    public class RelayEventArgs : EventArgs
    {
        public RelayEventArgs(Relay relay)
        {
            this.Relay = relay;
        }

        public Relay Relay { get; }
    }
}

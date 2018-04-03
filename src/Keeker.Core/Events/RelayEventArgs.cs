using Keeker.Core.Relays;
using System;

namespace Keeker.Core.Events
{
    public class RelayEventArgs : EventArgs
    {
        public RelayEventArgs(IRelay relay)
        {
            this.Relay = relay;
        }

        public IRelay Relay { get; }
    }
}

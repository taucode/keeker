using Keeker.Core.Relays;

namespace Keeker.Core.EventData
{
    public class RelayEventArgs : System.EventArgs
    {
        public RelayEventArgs(RelayBase relay)
        {
            this.Relay = relay;
        }

        public RelayBase Relay { get; }
    }
}

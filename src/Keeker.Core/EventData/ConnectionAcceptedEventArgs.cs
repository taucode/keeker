using System;
using System.Net.Sockets;

namespace Keeker.Core.EventData
{
    public class ConnectionAcceptedEventArgs : EventArgs
    {
        public ConnectionAcceptedEventArgs(TcpClient tcpClient)
        {
            this.TcpClient = tcpClient;
        }

        public TcpClient TcpClient { get; }
    }
}

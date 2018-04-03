using System;
using System.Net.Sockets;

namespace Keeker.Core.Events
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

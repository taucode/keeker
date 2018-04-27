using System;

namespace Keeker.Server
{
    public interface IHttpServer : IDisposable
    {
        void Start();

        string ListenedAddress { get; }

        string[] Hosts { get; }

        bool IsRunning { get; }

        event EventHandler<Connection> ConnectionAccepted;

        bool IsDisposed { get; }
    }
}

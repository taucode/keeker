using System;
using System.Net;

namespace Keeker.Core
{
    public interface IListener : IDisposable
    {
        void Start();

        bool IsStarted { get; }

        void Stop();

        IPEndPoint EndPoint { get; }

        event EventHandler Started;

        event EventHandler Stopped;
        
        event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;
    }
}

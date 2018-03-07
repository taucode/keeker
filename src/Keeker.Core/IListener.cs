using Keeker.Core.EventData;
using System;

namespace Keeker.Core
{
    public interface IListener : IDisposable
    {
        void Start();

        bool IsStarted { get; }

        void Stop();

        //IPEndPoint EndPoint { get; }

        //event EventHandler Started;

        //event EventHandler Stopped;
        
        event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        event EventHandler<RelayEventArgs> RelayCreated;
    }
}

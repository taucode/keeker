using Keeker.Core.Events;
using System;

namespace Keeker.Core.Listeners
{
    public interface IListener : IDisposable
    {
        string Id { get; }

        void Start();

        bool IsStarted { get; }

        void Stop();

        event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        event EventHandler<RelayEventArgs> RelayCreated;
    }
}

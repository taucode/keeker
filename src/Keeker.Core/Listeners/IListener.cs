using Keeker.Core.Events;
using Keeker.Core.Relays;
using System;
using System.IO;

namespace Keeker.Core.Listeners
{
    public interface IListener : IDisposable
    {
        string Id { get; }

        void Start();

        void Stop();

        bool IsRunning { get; }

        IRelay[] GetRelays();

        StreamWriter Log { get; set; }

        event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        event EventHandler<RelayEventArgs> RelayStarted;

        event EventHandler<RelayEventArgs> RelayDisposed;
    }
}

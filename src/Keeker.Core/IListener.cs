using Keeker.Core.EventData;
using System;

namespace Keeker.Core
{
    public interface IListener : IDisposable
    {
        string Id { get; }

        void Start();

        bool IsStarted { get; }

        void Stop();

        event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        event EventHandler<TheDeviceEventArgs> TheDeviceCreated;
    }
}

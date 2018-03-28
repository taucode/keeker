using System;

namespace Keeker.Core.Relays
{
    public interface IRelay : IDisposable
    {
        string ListenerId { get; }

        string Id { get; }

        void Start();

        void Stop();

        bool IsRunning { get; }

        string ExternalHostName { get; }

    }
}
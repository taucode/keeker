using System;

namespace Keeker.Convey.Listeners
{
    public interface IListener : IDisposable
    {
        void Start();

        void Stop();

        bool IsRunning { get; }
    }
}

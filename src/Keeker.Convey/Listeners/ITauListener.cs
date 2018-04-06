using System;

namespace Keeker.Convey.Listeners
{
    public interface ITauListener : IDisposable
    {
        void Start();

        void Stop();

        bool IsRunning { get; }
    }
}

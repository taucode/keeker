using System;

namespace Keeker.Convey.Proxies
{
    public interface IProxy : IDisposable
    {
        void Start();

        void Stop();

        bool IsRunning { get; }
    }
}

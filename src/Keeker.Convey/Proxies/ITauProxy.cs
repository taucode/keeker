using System;

namespace Keeker.Convey.Proxies
{
    public interface ITauProxy : IDisposable
    {
        void Start();

        void Stop();
    }
}

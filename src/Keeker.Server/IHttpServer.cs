using System;

namespace Keeker.Server
{
    public interface IHttpServer : IDisposable
    {
        void Start();

        bool IsRunning { get; }

        bool IsDisposed { get; }
    }
}

using System;
using System.IO;

namespace Keeker.Core.Listeners
{
    public interface IStreamListener : IDisposable
    {
        void Start();

        bool IsRunning { get; }

        string LocalEndpointName { get; }

        Stream AcceptStream();
    }
}

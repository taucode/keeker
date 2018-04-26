using System;
using System.IO;

namespace Keeker.Core.Listeners
{
    public class NetworkStreamListener : IStreamListener
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public bool IsRunning => throw new NotImplementedException();

        public string LocalEndpointName => throw new NotImplementedException();

        public Stream AcceptStream()
        {
            throw new NotImplementedException();
        }
    }
}

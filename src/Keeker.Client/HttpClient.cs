using Keeker.Core.Data;
using System;
using System.IO;

namespace Keeker.Client
{
    public class HttpClient : IHttpClient
    {
        #region IHttpClient Members

        public void Connect(Stream stream)
        {
            throw new NotImplementedException();
        }

        public Stream Stream { get; }

        public void Send(HttpRequestMetadata metadata, byte[] content)
        {
            throw new NotImplementedException();
        }

        public event Action<byte[]> RawDataReceived;

        public event Action<HttpResponseMetadata, byte[]> ResponseReceived;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

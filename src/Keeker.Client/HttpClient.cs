using Keeker.Core.Data;
using System;
using System.IO;

namespace Keeker.Client
{
    public class HttpClient : IHttpClient
    {
        #region Fields

        private readonly Stream _stream;

        #endregion

        public HttpClient(Stream stream)
        {
            // todo1[ak] checks

            _stream = stream;
        }

        #region IHttpClient Members

        public void Connect(Stream stream)
        {
            throw new NotImplementedException();
        }

        public Stream Stream => _stream;

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

using Keeker.Core.Data;
using System;
using System.IO;

namespace Keeker.Client
{
    public interface IHttpClient : IDisposable
    {
        void Connect(Stream stream);

        Stream Stream { get; }

        void Send(HttpRequestMetadata metadata, byte[] content);

        event Action<byte[]> RawDataReceived;

        event Action<HttpResponseMetadata, byte[]> ResponseReceived;
    }
}

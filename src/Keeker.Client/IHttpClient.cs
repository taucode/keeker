using System.Net;
using Keeker.Core.Data;

namespace Keeker.Client
{
    public interface IHttpClient
    {
        void Connect(IPEndPoint endPoint);

        void Send(HttpRequestMetadata metadata, byte[] content);
    }
}

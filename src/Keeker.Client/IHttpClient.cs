using System.Net;

namespace Keeker.Client
{
    public interface IHttpClient
    {
        void Connect(IPEndPoint endPoint);

    }
}

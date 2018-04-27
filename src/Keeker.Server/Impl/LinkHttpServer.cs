using Keeker.Core.Listeners;

namespace Keeker.Server.Impl
{
    public class LinkHttpServer : HttpServerBase
    {
        private readonly int _port;

        public LinkHttpServer(int port)
        {
            _port = port;
        }

        protected override IStreamListener CreateStreamListener()
        {
            return new LinkStreamListener(_port);
        }
    }
}

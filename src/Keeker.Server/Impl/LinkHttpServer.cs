using Keeker.Core.Listeners;

namespace Keeker.Server.Impl
{
    public class LinkHttpServer : HttpServerBase
    {
        private readonly int _port;

        public LinkHttpServer(string[] hosts, int port)
            : base(hosts)
        {
            _port = port;
        }

        protected override IStreamListener CreateStreamListener()
        {
            return new LinkStreamListener(_port);
        }

        protected override string GetListenedAddressImpl()
        {
            return $"link://{_port}";
        }
    }
}

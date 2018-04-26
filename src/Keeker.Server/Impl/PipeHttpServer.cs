using Keeker.Core.Listeners;

namespace Keeker.Server.Impl
{
    public class PipeHttpServer : HttpServerBase
    {
        private readonly int _port;

        public PipeHttpServer(int port)
        {
            _port = port;
        }

        protected override IStreamListener CreateStreamListener()
        {
            return new PipeStreamListener(_port);
        }
    }
}

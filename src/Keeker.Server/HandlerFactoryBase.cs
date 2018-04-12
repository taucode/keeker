using Keeker.Core.Data;
using Keeker.Server.Handlers;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Keeker.Server
{
    public class HandlerFactoryBase : IHandlerFactory
    {
        private readonly IStaticContentResolver _staticContentResolver;

        public HandlerFactoryBase(IStaticContentResolver staticContentResolver)
        {
            _staticContentResolver = staticContentResolver;
        }

        public IHandler CreateHandler(HttpRequestMetadata requestMetadata, Stream stream, ManualResetEvent stopSignal)
        {
            if (_staticContentResolver == null)
            {
                return this.CreateNotFoundHandler(requestMetadata, stream);
            }

            if (requestMetadata.Line.Method == HttpMethod.Get)
            {
                var info = _staticContentResolver.Resolve(requestMetadata.Line.RequestUri);

                if (info != null)
                {
                    var content = File.ReadAllText(info.FilePath, Encoding.UTF8);
                    return new StaticContentHandler(requestMetadata, stream, info.ContentType, Encoding.UTF8.GetBytes(content));
                }
            }

            return this.CreateNotFoundHandler(requestMetadata, stream);
        }

        private IHandler CreateNotFoundHandler(HttpRequestMetadata requestMetadata, Stream stream)
        {
            return new NotFoundHandler(requestMetadata, stream);
        }
    }
}

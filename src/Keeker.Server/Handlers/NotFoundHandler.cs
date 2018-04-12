using Keeker.Core.Data;
using Keeker.Core.Data.Builders;
using System.IO;
using System.Net;
using Keeker.Core;

namespace Keeker.Server.Handlers
{
    public class NotFoundHandler : ContentHandler
    {
        public NotFoundHandler(HttpRequestMetadata requestMetadata, Stream stream)
            : base(requestMetadata, stream)
        {
        }

        protected override HttpResponseMetadata CreateResponseMetadata()
        {
            var builder = new HttpResponseMetadataBuilder(HttpStatusCode.NotFound);
            builder.Headers.SetContentLength(0);
            return builder.Build();
        }

        protected override byte[] GenerateContent()
        {
            return new byte[0];
        }
    }
}

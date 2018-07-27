using Keeker.Core;
using Keeker.Core.Data;
using Keeker.Core.Data.Builders;
using System.IO;
using System.Net;

namespace Keeker.Server.Handlers
{
    public class NotFoundHandler : ContentHandler
    {
        public NotFoundHandler(string connectionId, HttpRequestMetadata requestMetadata, Stream stream)
            : base(connectionId, requestMetadata, stream)
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

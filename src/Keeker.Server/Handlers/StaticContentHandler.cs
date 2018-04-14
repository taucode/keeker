using Keeker.Core;
using Keeker.Core.Data;
using Keeker.Core.Data.Builders;
using System.IO;
using System.Net;

namespace Keeker.Server.Handlers
{
    public class StaticContentHandler : ContentHandler
    {
        public StaticContentHandler(
            HttpRequestMetadata requestMetadata,
            Stream stream,
            string contentType,
            byte[] content)
            : base(requestMetadata, stream)
        {
            this.ContentType = contentType;
            this.Content = content;
        }

        public byte[] Content { get; set; }

        public string ContentType { get; set; }

        protected override HttpResponseMetadata CreateResponseMetadata()
        {
            var builder = new HttpResponseMetadataBuilder(HttpStatusCode.OK);
            builder.Headers
                .SetContentType(this.ContentType)
                .SetContentLength(this.Content.Length);

            return builder.Build();
        }

        protected override byte[] GenerateContent()
        {
            return this.Content;
        }
    }
}

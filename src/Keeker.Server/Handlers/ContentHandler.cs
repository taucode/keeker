using Keeker.Core.Data;
using System.IO;

namespace Keeker.Server.Handlers
{
    public abstract class ContentHandler : IHandler
    {
        private readonly Stream _stream;

        protected ContentHandler(HttpRequestMetadata requestMetadata, Stream stream)
        {
            this.RequestMetadata = requestMetadata;
            _stream = stream;
        }

        protected HttpRequestMetadata RequestMetadata { get; }

        public void Handle()
        {
            var responseMetadata = this.CreateResponseMetadata();
            var responseMetadataBytes = responseMetadata.Serialize();

            _stream.Write(responseMetadataBytes, 0, responseMetadataBytes.Length);

            var content = this.GenerateContent();

            _stream.Write(content, 0, content.Length);
        }

        protected abstract HttpResponseMetadata CreateResponseMetadata();
        protected abstract byte[] GenerateContent();
    }
}

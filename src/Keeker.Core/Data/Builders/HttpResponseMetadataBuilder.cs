using System.Net;

namespace Keeker.Core.Data.Builders
{
    public class HttpResponseMetadataBuilder
    {
        public HttpResponseMetadataBuilder()
        {
            this.Line = new HttpStatusLineBuilder();
            this.Headers = new HttpHeaderCollectionBuilder();
        }

        public HttpResponseMetadataBuilder(HttpStatusCode code)
        {
            this.Line = new HttpStatusLineBuilder(code);
            this.Headers = new HttpHeaderCollectionBuilder();
        }

        public HttpResponseMetadataBuilder(HttpResponseMetadata responseMetadata)
        {
            this.Line = new HttpStatusLineBuilder(responseMetadata.Line);
            this.Headers = new HttpHeaderCollectionBuilder(responseMetadata.Headers);
        }

        public HttpStatusLineBuilder Line { get; set; }

        public HttpHeaderCollectionBuilder Headers { get; set; }

        public HttpResponseMetadata Build()
        {
            return new HttpResponseMetadata(
                this.Line.Build(),
                this.Headers.Build());
        }

    }
}
namespace Keeker.Core
{
    public class HttpRequestMetadataBuilder
    {
        public HttpRequestMetadataBuilder()
        {
            this.Line = new HttpRequestLineBuilder();
            this.Headers = new HttpHeaderCollectionBuilder();
        }

        public HttpRequestMetadataBuilder(HttpRequestMetadata requestMetadata)
        {
            this.Line = new HttpRequestLineBuilder(requestMetadata.Line);
            this.Headers = new HttpHeaderCollectionBuilder(requestMetadata.Headers);
        }

        public HttpRequestLineBuilder Line { get; set; }

        public HttpHeaderCollectionBuilder Headers { get; set; }

        public HttpRequestMetadata Build()
        {
            return new HttpRequestMetadata(
                this.Line.Build(),
                this.Headers.Build());
        }
    }
}
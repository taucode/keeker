namespace Keeker.Convey.Data.Builders
{
    public class HttpRequestMetadataBuilder
    {
        public HttpRequestMetadataBuilder()
        {
            this.Line = new Convey.Data.Builders.HttpRequestLineBuilder();
            this.Headers = new Convey.Data.Builders.HttpHeaderCollectionBuilder();
        }

        public HttpRequestMetadataBuilder(HttpRequestMetadata requestMetadata)
        {
            this.Line = new Convey.Data.Builders.HttpRequestLineBuilder(requestMetadata.Line);
            this.Headers = new Convey.Data.Builders.HttpHeaderCollectionBuilder(requestMetadata.Headers);
        }

        public Convey.Data.Builders.HttpRequestLineBuilder Line { get; set; }

        public Convey.Data.Builders.HttpHeaderCollectionBuilder Headers { get; set; }

        public HttpRequestMetadata Build()
        {
            return new HttpRequestMetadata(
                this.Line.Build(),
                this.Headers.Build());
        }
    }
}
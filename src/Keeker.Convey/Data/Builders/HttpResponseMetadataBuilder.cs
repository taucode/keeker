namespace Keeker.Convey.Data.Builders
{
    public class HttpResponseMetadataBuilder
    {
        public HttpResponseMetadataBuilder()
        {
            this.Line = new HttpStatusLineBuilder();
            this.Headers = new Convey.Data.Builders.HttpHeaderCollectionBuilder();
        }

        public HttpResponseMetadataBuilder(HttpResponseMetadata responseMetadata)
        {
            this.Line = new HttpStatusLineBuilder(responseMetadata.Line);
            this.Headers = new Convey.Data.Builders.HttpHeaderCollectionBuilder(responseMetadata.Headers);
        }

        public HttpStatusLineBuilder Line { get; set; }

        public Convey.Data.Builders.HttpHeaderCollectionBuilder Headers { get; set; }

        public HttpResponseMetadata Build()
        {
            return new HttpResponseMetadata(
                this.Line.Build(),
                this.Headers.Build());
        }

    }
}
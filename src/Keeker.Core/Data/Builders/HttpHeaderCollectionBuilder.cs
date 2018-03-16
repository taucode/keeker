using System.Collections.Generic;
using System.Linq;

namespace Keeker.Core.Data.Builders
{
    public class HttpHeaderCollectionBuilder
    {
        public HttpHeaderCollectionBuilder()
        {
            this.Headers = new List<HttpHeaderBuilder>();
        }

        public HttpHeaderCollectionBuilder(HttpHeaderCollection headers)
        {
            this.Headers = headers
                .Select(x => new HttpHeaderBuilder(x))
                .ToList();
        }

        public void Replace(string name, string value)
        {
            var header = Headers.Single(x => x.Name == name);
            header.Value = value;
        }

        public List<HttpHeaderBuilder> Headers { get; }

        public HttpHeaderCollection Build()
        {
            return new HttpHeaderCollection(this.Headers
                .Select(x => x.Build()));
        }
    }
}
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
            var header = Headers.SingleOrDefault(x => x.Name == name);

            if (header == null)
            {
                Headers.Add(new HttpHeaderBuilder(name, value));
            }
            else
            {
                header.Value = value;
            }
        }

        public List<HttpHeaderBuilder> Headers { get; }

        public HttpHeaderCollection Build()
        {
            return new HttpHeaderCollection(this.Headers
                .Select(x => x.Build()));
        }
    }
}
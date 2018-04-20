using System.Net.Http;

namespace Keeker.Core.Data.Builders
{
    public class HttpRequestLineBuilder
    {
        public HttpRequestLineBuilder()
        {
            this.Method = HttpMethod.Get;
            this.RequestUri = "/";
            this.Version = CoreHelper.HttpVersion11;
        }

        public HttpRequestLineBuilder(string uri)
        {
            this.Method = HttpMethod.Get;
            this.RequestUri = uri;
            this.Version = CoreHelper.HttpVersion11;
        }

        public HttpRequestLineBuilder(HttpMethod method, string uri)
        {
            this.Method = method;
            this.RequestUri = uri;
            this.Version = CoreHelper.HttpVersion11;
        }

        public HttpRequestLineBuilder(HttpRequestLine line)
        {
            this.Method = line.Method;
            this.RequestUri = line.RequestUri;
            this.Version = line.Version;
        }

        public HttpMethod Method { get; set; }
        public string RequestUri { get; set; }
        public string Version { get; set; }

        public override string ToString()
        {
            return $"{this.Method} {this.RequestUri} {this.Version}{CoreHelper.CrLf}";
        }

        public HttpRequestLine Build()
        {
            return new HttpRequestLine(this.Method, this.RequestUri, this.Version);
        }
    }
}
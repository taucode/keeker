using System.Net.Http;

namespace Keeker.Convey.Data.Builders
{
    public class HttpRequestLineBuilder
    {
        public HttpRequestLineBuilder()
        {   
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
            return $"{this.Method} {this.RequestUri} {this.Version}{Helper.CrLf}";
        }

        public HttpRequestLine Build()
        {
            return new HttpRequestLine(this.Method, this.RequestUri, this.Version);
        }
    }
}
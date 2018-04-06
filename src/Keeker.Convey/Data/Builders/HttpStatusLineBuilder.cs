using System.Net;

namespace Keeker.Convey.Data.Builders
{
    public class HttpStatusLineBuilder
    {
        public HttpStatusLineBuilder()
        {
        }
        
        public HttpStatusLineBuilder(HttpStatusLine line)
        {
            this.Version = line.Version;
            this.Code = line.Code;
            this.Reason = line.Reason;
        }

        public string Version { get; set; }

        public HttpStatusCode Code { get; set; }

        public string Reason { get; set; }

        public override string ToString()
        {
            return $"{this.Version} {(int)this.Code} {this.Reason}";
        }

        public HttpStatusLine Build()
        {
            return new HttpStatusLine(this.Version, this.Code, this.Reason);
        }
    }
}
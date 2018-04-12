using System.Net;

namespace Keeker.Core.Data.Builders
{
    public class HttpStatusLineBuilder
    {
        public HttpStatusLineBuilder()
        {
        }

        public HttpStatusLineBuilder(string version, HttpStatusCode code, string reason)
        {
            this.Version = version;
            this.Code = code;
            this.Reason = reason;
        }

        public HttpStatusLineBuilder(HttpStatusCode code)
        {
            this.Version = CoreHelper.HttpVersion11;
            this.Code = code;
            this.Reason = code.ToReason();
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
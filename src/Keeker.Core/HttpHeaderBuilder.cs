namespace Keeker.Core
{
    public class HttpHeaderBuilder
    {
        public HttpHeaderBuilder()
        {   
        }

        public HttpHeaderBuilder(HttpHeader header)
        {
            this.Name = header.Name;
            this.Value = header.Value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"{this.Name}: {this.Value}{HttpHelper.CrLf}";
        }

        public HttpHeader Build()
        {
            return new HttpHeader(this.Name, this.Value);
        }
    }
}
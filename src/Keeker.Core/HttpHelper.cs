using System.Linq;

namespace Keeker.Core
{
    public static class HttpHelper
    {
        internal const string CrLf = "\r\n";
        internal static readonly byte[] CrLfBytes = CrLf.ToAsciiBytes();
        internal const string CrLfCrLf = CrLf + CrLf;
        internal static readonly byte[] CrLfCrLfBytes = CrLfCrLf.ToAsciiBytes();

        public static HttpTransferEncoding GetTransferEncoding(this HttpHeaderCollection httpHeaders)
        {
            return httpHeaders.Single(x => x.Name == "Transfer-Encoding").Value.ToEnum<HttpTransferEncoding>();
        }

        public static int GetContentLength(this HttpHeaderCollection httpHeaders)
        {
            return httpHeaders.Single(x => x.Name == "Content-Length").Value.ToInt32();
        }

    }
}

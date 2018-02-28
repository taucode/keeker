namespace Keeker.Core
{
    internal static class HttpHelper
    {
        internal const string CrLf = "\r\n";
        internal static readonly byte[] CrLfBytes = CrLf.ToAsciiBytes();
        internal const string CrLfCrLf = CrLf + CrLf;
        internal static readonly byte[] CrLfCrLfBytes = CrLfCrLf.ToAsciiBytes();
    }
}

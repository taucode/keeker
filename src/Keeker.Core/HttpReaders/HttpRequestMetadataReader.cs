using Keeker.Core.Data;
using Keeker.Core.Streams;
using System.Threading;

namespace Keeker.Core.HttpReaders
{
    public class HttpRequestMetadataReader : HttpMetadataReaderBase<HttpRequestMetadata>
    {
        public HttpRequestMetadataReader(KeekStream stream, ManualResetEvent stopSignal)
            : base(stream, stopSignal)
        {
        }

        protected override HttpRequestMetadata Parse(byte[] buffer, int start)
        {
            return HttpRequestMetadata.Parse(buffer, start);
        }
    }
}

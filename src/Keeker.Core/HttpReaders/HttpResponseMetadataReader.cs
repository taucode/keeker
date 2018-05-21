using Keeker.Core.Data;
using Keeker.Core.Streams;
using System.Threading;

namespace Keeker.Core.HttpReaders
{
    public class HttpResponseMetadataReader : HttpMetadataReaderBase<HttpResponseMetadata>
    {
        public HttpResponseMetadataReader(KeekStream stream, ManualResetEvent stopSignal)
            : base(stream, stopSignal)
        {
        }

        protected override HttpResponseMetadata Parse(byte[] buffer, int start)
        {
            return HttpResponseMetadata.Parse(buffer, start);
        }
    }
}

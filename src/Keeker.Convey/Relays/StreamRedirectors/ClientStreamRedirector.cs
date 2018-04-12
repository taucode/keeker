using Keeker.Convey.Relays.ContentRedirectors;
using Keeker.Core.Data;
using Keeker.Core.Data.Builders;
using Keeker.Core.Exceptions;
using Keeker.Core.Streams;
using System.IO;
using Keeker.Core;

namespace Keeker.Convey.Relays.StreamRedirectors
{
    public class ClientStreamRedirector : StreamRedirector<HttpRequestMetadata>
    {
        private readonly string _host;
        private readonly string _targetHost;

        public ClientStreamRedirector(
            Relay relay,
            KeekStream sourceStream,
            Stream destinationStream,
            string host,
            string targetHost)
            : base(
                relay,
                sourceStream,
                destinationStream)
        {
            _host = host;
            _targetHost = targetHost;
        }

        protected override HttpRequestMetadata ParseMetadata(byte[] buffer, int start)
        {
            return HttpRequestMetadata.Parse(buffer, start);
        }

        protected override void CheckMetadata(HttpRequestMetadata metadata)
        {
            if (metadata.Headers.GetHost() != _host)
            {
                throw new BadHttpDataException("Unexpected host");
            }
        }

        protected override HttpRequestMetadata TransformMetadata(HttpRequestMetadata metadata)
        {
            var requestMetadataBuilder = new HttpRequestMetadataBuilder(metadata);
            requestMetadataBuilder.Headers.Replace("Host", _targetHost);
            var transformedRequestMetadata = requestMetadataBuilder.Build();

            return transformedRequestMetadata;
        }

        protected override ContentRedirector ResolveDataRedirector(HttpRequestMetadata metadata)
        {
            ContentRedirector dataRedirector = null;
            if (metadata.Headers.ContainsName("Content-Length"))
            {
                var length = metadata.Headers.GetContentLength();
                dataRedirector = new FixedSizeContentRedirector(
                    this.Relay.StopSignal,
                    this.SourceStream,
                    this.SourceBuffer,
                    this.DestinationStream,
                    length);
            }

            return dataRedirector;
        }
    }
}

using Keeker.Convey.Data;
using Keeker.Convey.Data.Builders;
using Keeker.Convey.Relays.ContentRedirectors;
using Keeker.Convey.Streams;
using System;
using System.IO;
using System.Threading;

namespace Keeker.Convey.Relays.StreamRedirectors
{
    public class ClientStreamRedirector : StreamRedirector<HttpRequestMetadata>
    {
        private readonly string _host;
        private readonly string _targetHost;

        public ClientStreamRedirector(
            ManualResetEvent stopSignal,
            KeekStream sourceStream,
            Stream destinationStream,
            string host,
            string targetHost)
            : base(
                  stopSignal,
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
                throw new ApplicationException(); // todo0[ak] what to do?
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
                    this.StopSignal,
                    this.SourceStream,
                    this.SourceBuffer,
                    this.DestinationStream,
                    length);
            }

            return dataRedirector;
        }
    }
}

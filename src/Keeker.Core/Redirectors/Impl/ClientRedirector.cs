using Keeker.Core.Data;
using Keeker.Core.Data.Builders;
using Keeker.Core.Redirectors.DataRedirectors;
using Keeker.Core.Streams;
using System;
using System.IO;

namespace Keeker.Core.Redirectors.Impl
{
    public class ClientRedirector : Redirector<HttpRequestMetadata>
    {
        private readonly string _host;
        private readonly string _targetHost;

        public ClientRedirector(
            KeekStream sourceStream,
            Stream destinationStream,
            string host,
            string targetHost)
            : base(sourceStream, destinationStream)
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

        protected override DataRedirector ResolveDataRedirector(HttpRequestMetadata metadata)
        {
            DataRedirector dataRedirector = null;
            if (metadata.Headers.ContainsName("Content-Length"))
            {
                var length = metadata.Headers.GetContentLength();
                dataRedirector = new FixedSizeDataRedirector(
                    this.SourceStream,
                    this.SourceBuffer,
                    this.DestinationStream,
                    length);
            }

            return dataRedirector;
        }
    }
}

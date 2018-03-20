using Keeker.Core.Data;
using Keeker.Core.Data.Builders;
using Keeker.Core.Redirectors.DataRedirectors;
using Keeker.Core.Streams;
using System;
using System.IO;
using System.Net;

namespace Keeker.Core.Redirectors.Impl
{
    public class ServerRedirector : Redirector<HttpResponseMetadata>
    {
        private readonly string _protocol;
        private readonly string _externalHostName;
        private readonly string _domesticAuthority;
        private readonly string _domesticAuthorityWithPort;

        public ServerRedirector(
            KeekStream sourceStream,
            Stream destinationStream,
            string protocol,
            string externalHostName,
            string domesticAuthority,
            string domesticAuthorityWithPort)
            : base(sourceStream, destinationStream)
        {
            _protocol = protocol;
            _externalHostName = externalHostName;
            _domesticAuthority = domesticAuthority;
            _domesticAuthorityWithPort = domesticAuthorityWithPort;
        }

        protected override HttpResponseMetadata ParseMetadata(byte[] buffer, int start)
        {
            return HttpResponseMetadata.Parse(buffer, start);
        }

        protected override void CheckMetadata(HttpResponseMetadata metadata)
        {
            // idle
        }

        protected override HttpResponseMetadata TransformMetadata(HttpResponseMetadata metadata)
        {
            if (metadata.Line.Code == HttpStatusCode.Found)
            {
                var location = metadata.Headers.GetLocation();

                var locationIsAbsolute = location.StartsWith("http://") || location.StartsWith("https://");
                if (locationIsAbsolute)
                {
                    var uri = new Uri(location);

                    if (
                        uri.Authority == _domesticAuthority ||
                        uri.Authority == _domesticAuthorityWithPort)
                    {
                        var changedLocation = this.BuildExternalUrl(uri.PathAndQuery);

                        var responseMetadataBuilder = new HttpResponseMetadataBuilder(metadata);
                        responseMetadataBuilder.Headers.Replace("Location", changedLocation);

                        metadata = responseMetadataBuilder.Build();
                    }
                }
            }

            return metadata;
        }

        protected override DataRedirector ResolveDataRedirector(HttpResponseMetadata metadata)
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
            else if (metadata.Headers.ContainsName("Transfer-Encoding"))
            {
                var transferEncoding = metadata.Headers.GetTransferEncoding();
                if (transferEncoding == HttpTransferEncoding.Chunked)
                {
                    dataRedirector = new ChunkedDataRedirector(
                        this.SourceStream,
                        this.SourceBuffer,
                        this.DestinationStream);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return dataRedirector;
        }

        private string BuildExternalUrl(string pathAndQuery)
        {
            return $"{_protocol}://{_externalHostName}{pathAndQuery}";
        }
    }
}

using Keeker.Core;
using Keeker.Core.Data;
using Keeker.Core.HttpReaders;
using Keeker.Core.Streams;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Keeker.Client
{
    public class HttpClient : IHttpClient
    {
        #region Fields

        private readonly KeekStream _stream;
        private readonly HttpResponseMetadataReader _metadataReader;
        private readonly Task _readingTask;
        private readonly ManualResetEvent _stopSignal;

        #endregion

        #region Constructor

        public HttpClient(Stream stream)
        {
            // todo1[ak] checks

            _stream = new KeekStream(stream, true);
            _stopSignal = new ManualResetEvent(false);
            _metadataReader = new HttpResponseMetadataReader(_stream, _stopSignal);
            _readingTask = new Task(this.ReadingRoutine);
            _readingTask.Start();
        }

        #endregion

        #region Private

        private void ReadingRoutine()
        {
            while (true)
            {
                var metadata = _metadataReader.Read();

                if (metadata.Headers.ContainsName("Content-Length"))
                {
                    byte[] content;

                    var contentLength = metadata.Headers.GetContentLength();
                    if (contentLength > 0)
                    {
                        content = new byte[contentLength];
                        _stream.Read(content, 0, contentLength);
                    }
                    else
                    {
                        content = new byte[0];
                    }

                    this.ResponseReceived?.Invoke(metadata, content);
                }
                else
                {
                    throw new NotImplementedException();
                }
                
                // todo00[ak] try/catch!
                // todo0000000[ak]never ends!
            }
        }

        #endregion

        #region IHttpClient Members

        public Stream Stream => _stream;

        public void Send(HttpRequestMetadata metadata, byte[] content)
        {
            var metadataBuffer = metadata.Serialize();
            _stream.Write(metadataBuffer, 0, metadataBuffer.Length);
            _stream.Write(content, 0, content.Length);
        }

        public event Action<byte[]> RawDataReceived;

        public event Action<HttpResponseMetadata, byte[]> ResponseReceived;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

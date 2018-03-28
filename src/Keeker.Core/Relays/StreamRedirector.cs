using Keeker.Core.Data;
using Keeker.Core.Streams;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Keeker.Core.Relays
{
    public abstract class StreamRedirector<TMetadata> where TMetadata : IHttpMetadata
    {
        private const int PORTION_SIZE = 65530;
        private const int IDLE_TIMEOUT_MILLISECONDS = 1;

        private StreamRedirectorState _state;

        private readonly TimeSpan _idleTimeout;
        private TMetadata _lastMetadata;

        protected StreamRedirector(
            Relay relay,
            KeekStream sourceStream,
            Stream destinationStream)
        {
            this.Relay = relay;
            _idleTimeout = TimeSpan.FromMilliseconds(IDLE_TIMEOUT_MILLISECONDS);

            this.SourceStream = sourceStream;
            this.SourceBuffer = new AutoBuffer();

            this.DestinationStream = destinationStream;
        }

        protected Relay Relay { get; private set; }

        protected KeekStream SourceStream { get; private set; }
        protected AutoBuffer SourceBuffer { get; private set; }
        protected Stream DestinationStream { get; private set; }

        protected abstract TMetadata ParseMetadata(byte[] buffer, int start);
        protected abstract void CheckMetadata(TMetadata metadata);
        protected abstract TMetadata TransformMetadata(TMetadata metadata);
        protected abstract ContentRedirector ResolveDataRedirector(TMetadata metadata);

        private void DisposeRelay(object dummy)
        {
            try
            {
                this.Relay.Dispose();
            }
            catch
            {   
            }
        }

        private void DoIdle()
        {
            if (this.SourceStream.AccumulatedBytesCount > 0)
            {
                _state = StreamRedirectorState.Metadata;
                return;
            }

            var bytesReadCount = this.SourceStream.ReadInnerStream(PORTION_SIZE);

            if (bytesReadCount == 0)
            {
                var gotSignal = this.Relay.Signal.WaitOne(_idleTimeout);
                if (gotSignal)
                {
                    _state = StreamRedirectorState.Stop;
                }
            }
            else
            {
                _state = StreamRedirectorState.Metadata;
            }
        }

        private int GetMetadataLength()
        {
            var index = this.SourceStream.PeekIndexOf(HttpHelper.CrLfCrLfBytes);
            if (index == -1)
            {
                return -1;
            }

            var metadataLength = index + HttpHelper.CrLfCrLfBytes.Length;
            return metadataLength;
        }

        private void DoMetadata()
        {
            if (this.SourceStream.AccumulatedBytesCount == 0)
            {
                // let's accumulate some data
                var bytesReadCount = this.SourceStream.ReadInnerStream(PORTION_SIZE);

                if (bytesReadCount == 0)
                {
                    _state = StreamRedirectorState.Idle;
                    return;
                }
            }

            var metadataLength = this.GetMetadataLength();

            if (metadataLength >= 0)
            {
                this.SourceBuffer.Allocate(metadataLength);
                this.SourceStream.Read(this.SourceBuffer.Raw, 0, metadataLength);

                var metadata = this.ParseMetadata(this.SourceBuffer.Raw, 0);
                this.CheckMetadata(metadata);
                var transformedMetadata = this.TransformMetadata(metadata);
                var transformedMetadataBytes = transformedMetadata.Serialize();
                this.DestinationStream.Write(transformedMetadataBytes, 0, transformedMetadataBytes.Length);

                _lastMetadata = transformedMetadata;
                _state = StreamRedirectorState.Data;
            }
            else
            {
                // seems like not enough bytes to extract metadata, read more
                this.SourceStream.ReadInnerStream(PORTION_SIZE);
            }
        }

        private void DoData()
        {
            var dataRedirector = this.ResolveDataRedirector(_lastMetadata);
            dataRedirector?.Redirect();

            _state = StreamRedirectorState.Idle;
        }

        private void Routine()
        {
            _state = StreamRedirectorState.Idle;

            try
            {
                var goOn = true;
                while (goOn)
                {
                    switch (_state)
                    {
                        case StreamRedirectorState.Idle:
                            this.DoIdle();
                            break;

                        case StreamRedirectorState.Metadata:
                            this.DoMetadata();
                            break;

                        case StreamRedirectorState.Data:
                            this.DoData();
                            break;

                        case StreamRedirectorState.Stop:
                            goOn = false;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception)
            {
                // todo1[ak] log or something

                this.Relay.Signal.Set();
            }

            ThreadPool.QueueUserWorkItem(this.DisposeRelay);
        }

        public void Start()
        {
            // todo2[ak] check not start twice(?)

            new Task(this.Routine).Start();
        }
    }
}

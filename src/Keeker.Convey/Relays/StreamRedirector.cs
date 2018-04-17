using Keeker.Convey.Logging;
using Keeker.Core.Data;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Keeker.Core;
using Keeker.Core.Streams;

namespace Keeker.Convey.Relays
{
    public abstract class StreamRedirector<TMetadata> where TMetadata : IHttpMetadata
    {
        #region Logging

        private ILog Logger { get; } = LogProvider.GetCurrentClassLogger();

        #endregion

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
            this.Task = new Task(this.Routine);
        }

        protected Relay Relay { get; private set; }
        protected KeekStream SourceStream { get; private set; }
        protected AutoBuffer SourceBuffer { get; private set; }
        protected Stream DestinationStream { get; private set; }
        protected Task Task { get; private set; }

        protected abstract TMetadata ParseMetadata(byte[] buffer, int start);
        protected abstract void CheckMetadata(TMetadata metadata);
        protected abstract TMetadata TransformMetadata(TMetadata metadata);
        protected abstract ContentRedirector ResolveDataRedirector(TMetadata metadata);
        
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
                var gotSignal = this.Relay.StopSignal.WaitOne(_idleTimeout);
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
            var index = this.SourceStream.PeekIndexOf(CoreHelper.CrLfCrLfBytes);
            if (index == -1)
            {
                return -1;
            }

            var metadataLength = index + CoreHelper.CrLfCrLfBytes.Length;
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

                //Logger.Info(transformedMetadataBytes.ToAsciiString());

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

                Logger.Info("Stopped due to request");

            }
            catch (Exception ex)
            {
                // todo1[ak] log or something
                Logger.InfoFormat("{0} Stopped due to an exception {1}", this.GetType().FullName, ex.GetType().FullName);
                this.Relay.StopSignal.Set();
            }

            ThreadPool.QueueUserWorkItem(x => this.Relay.Dispose());
        }

        public void Start()
        {
            this.Task.Start();

            Logger.InfoFormat("Started redirector: {0}{1}", this.Relay.Id, this.GetType().Name);
        }

        public void Wait()
        {
            this.Task.Wait();
        }
    }
}

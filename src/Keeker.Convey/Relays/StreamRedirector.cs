using Keeker.Convey.Data;
using Keeker.Convey.Logging;
using Keeker.Convey.Streams;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Keeker.Convey.Relays
{
    public abstract class StreamRedirector<TMetadata> where TMetadata : IHttpMetadata
    {
        #region Logging

        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        #endregion

        private const int PORTION_SIZE = 65530;
        private const int IDLE_TIMEOUT_MILLISECONDS = 1;

        private StreamRedirectorState _state;

        private readonly TimeSpan _idleTimeout;
        private TMetadata _lastMetadata;

        protected StreamRedirector(
            ManualResetEvent stopSignal,
            KeekStream sourceStream,
            Stream destinationStream)
        {
            this.StopSignal = stopSignal;
            _idleTimeout = TimeSpan.FromMilliseconds(IDLE_TIMEOUT_MILLISECONDS);

            this.SourceStream = sourceStream;
            this.SourceBuffer = new AutoBuffer();

            this.DestinationStream = destinationStream;
            this.Task = new Task(this.Routine);
        }

        protected ManualResetEvent StopSignal { get; private set; }

        protected KeekStream SourceStream { get; private set; }
        protected AutoBuffer SourceBuffer { get; private set; }
        protected Stream DestinationStream { get; private set; }
        protected Task Task { get; private set; }

        protected abstract TMetadata ParseMetadata(byte[] buffer, int start);
        protected abstract void CheckMetadata(TMetadata metadata);
        protected abstract TMetadata TransformMetadata(TMetadata metadata);
        protected abstract ContentRedirector ResolveDataRedirector(TMetadata metadata);

        //private void DisposeRelay(object dummy)
        //{
        //    try
        //    {
        //        this.Relay.Dispose();
        //    }
        //    catch
        //    {   
        //    }
        //}

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
                var gotSignal = this.StopSignal.WaitOne(_idleTimeout);
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
            var index = this.SourceStream.PeekIndexOf(TauHelper.CrLfCrLfBytes);
            if (index == -1)
            {
                return -1;
            }

            var metadataLength = index + TauHelper.CrLfCrLfBytes.Length;
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

                throw new NotImplementedException(); // todo000[ak]
            }
            catch (Exception ex)
            {
                // todo1[ak] log or something
                Logger.InfoException("{0} stopped due to an exception", ex);
                this.StopSignal.Set();
            }
        }

        public void Start()
        {
            this.Task.Start();
        }
    }
}

using Keeker.Core.Data;
using Keeker.Core.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Keeker.Server
{
    public class Connection : IDisposable
    {
        private const int TIMEOUT_MILLISECONDS = 1;

        private bool _isRunning;
        private bool _isDisposed;
        private readonly object _lock;

        private readonly KeekStream _stream;
        private readonly RequestMetadataReader _requestMetadataReader;

        private readonly IHandlerFactory _handlerFactory;

        private readonly Task _requestTask;
        private readonly Task _responseTask;

        private readonly ManualResetEvent _stopSignal;
        private readonly AutoResetEvent _newTaskArrivedSignal;
        private readonly TimeSpan _timeout;

        private readonly Queue<Task> _tasks;

        public Connection(string id, Stream innerStream, IHandlerFactory handlerFactory)
        {
            _lock = new object();
            
            _stream = new KeekStream(innerStream, false);
            _requestTask = new Task(this.RequestRoutine);
            _responseTask = new Task(this.ResponseRoutine);
            _stopSignal = new ManualResetEvent(false);
            _handlerFactory = handlerFactory;

            _requestMetadataReader = new RequestMetadataReader(_stream, _stopSignal);
            _tasks = new Queue<Task>();

            _newTaskArrivedSignal = new AutoResetEvent(false);
            _timeout = TimeSpan.FromMilliseconds(TIMEOUT_MILLISECONDS);

            this.Id = id;
        }

        private void RequestRoutine()
        {
            while (true)
            {
                var metadata = _requestMetadataReader.Read();
                var handler = this.ResolveHandler(metadata);

                var task = new Task(handler.Handle);
                lock (_lock)
                {
                    _tasks.Enqueue(task);
                }
            }
        }

        private IHandler ResolveHandler(HttpRequestMetadata metadata)
        {
            var handler = _handlerFactory.CreateHandler(metadata, _stream, _stopSignal);
            return handler;
        }

        private void ResponseRoutine()
        {
            while (true)
            {
                var gotStopSignal = _stopSignal.WaitOne(0);
                if (gotStopSignal)
                {
                    break;
                }

                Task currentTask = null;

                lock (_lock)
                {
                    if (_tasks.Count > 0)
                    {
                        currentTask = _tasks.Dequeue();
                    }
                    else
                    {
                        _newTaskArrivedSignal.WaitOne(_timeout);
                        continue;
                    }
                }

                currentTask.Start();
                currentTask.Wait();
            }
        }

        public string Id { get; }

        public void Start()
        {
            lock (_lock)
            {
                if (_isRunning)
                {
                    throw new InvalidOperationException("Connection already running");
                }

                if (_isDisposed)
                {
                    throw new ObjectDisposedException("Connection");
                }

                _isRunning = true;
            }

            _requestTask.Start();
            _responseTask.Start();
        }

        public bool IsRunning
        {
            get
            {
                lock (_lock)
                {
                    return _isRunning;
                }
            }
        }

        public bool IsDisposed
        {
            get
            {
                lock (_lock)
                {
                    return _isDisposed;
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

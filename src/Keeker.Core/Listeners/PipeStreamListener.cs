using Keeker.Core.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Keeker.Core.Listeners
{
    public class PipeStreamListener : IStreamListener
    {
        #region Constants

        private const int TIMEOUT_MILLISECONDS = 1;

        #endregion

        #region Nested

        private class ObjectAddress : IEquatable<ObjectAddress>
        {
            private readonly object _obj;

            public ObjectAddress(object obj)
            {
                _obj = obj ?? throw new ArgumentNullException(nameof(obj));
            }


            public bool Equals(ObjectAddress other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return ReferenceEquals(_obj, other._obj);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((ObjectAddress)obj);
            }

            public override int GetHashCode()
            {
                return _obj.GetHashCode();
            }
        }

        #endregion

        #region Static

        private static readonly HashSet<ObjectAddress> _usedPipes;
        private static readonly object _staticLock;

        private static readonly Dictionary<int, PipeStreamListener> _runningListeners;

        static PipeStreamListener()
        {
            _staticLock = new object();
            _usedPipes = new HashSet<ObjectAddress>();
            _runningListeners = new Dictionary<int, PipeStreamListener>();
        }

        private static PipeStreamListener GetRunningListener(int port)
        {
            lock (_staticLock)
            {
                _runningListeners.TryGetValue(port, out var listener);
                return listener;
            }
        }

        private static void RegisterRunningListener(PipeStreamListener listener)
        {
            lock (_staticLock)
            {
                _runningListeners.Add(listener.Port, listener);
            }
        }

        public static void Connect(int port, Pipe pipe)
        {
            lock (_staticLock)
            {
                var address = new ObjectAddress(pipe);
                if (_usedPipes.Contains(address))
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                var listener = GetRunningListener(port);

                if (listener == null)
                {
                    throw new ApplicationException("Connection refused"); // todo2[ak] connection refused
                }

                _usedPipes.Add(new ObjectAddress(pipe));
                listener.QueuePipeForAcceptance(pipe);
            }
        }

        #endregion

        #region Fields

        private readonly object _lock;
        private bool _isDisposed;
        private readonly Queue<Pipe> _pipes;
        private readonly AutoResetEvent _signal;

        #endregion

        #region Constructor

        public PipeStreamListener(int port)
        {
            this.Port = port;
            _pipes = new Queue<Pipe>();
            _signal = new AutoResetEvent(false);
            _lock = new object();
        }

        #endregion

        #region Private

        private void QueuePipeForAcceptance(Pipe pipe)
        {
            lock (_lock)
            {
                _pipes.Enqueue(pipe);
                _signal.Set();
            }
        }

        #endregion

        #region Public

        public int Port { get; }

        #endregion

        #region IStreamListener Members

        public void Start()
        {
            lock (_lock)
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().Name);
                }
            }

            RegisterRunningListener(this);
        }

        public bool IsRunning
        {
            get
            {
                var listener = GetRunningListener(this.Port);
                return ReferenceEquals(this, listener);
            }
        }

        public string LocalEndpointName => $"pipe:{this.Port}";

        public Stream AcceptStream()
        {
            while (true)
            {
                lock (_lock)
                {
                    if (_isDisposed)
                    {
                        throw new ObjectDisposedException(this.GetType().Name);
                    }

                    if (_pipes.Count > 0)
                    {
                        var pipe = _pipes.Dequeue();
                        return pipe.Stream2;
                    }

                    _signal.WaitOne(TIMEOUT_MILLISECONDS);
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

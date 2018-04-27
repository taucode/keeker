using Keeker.Core.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Keeker.Core.Listeners
{
    public class LinkStreamListener : IStreamListener
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

        private static readonly HashSet<ObjectAddress> _usedLinks;
        private static readonly object _staticLock;

        private static readonly Dictionary<int, LinkStreamListener> _runningListeners;

        static LinkStreamListener()
        {
            _staticLock = new object();
            _usedLinks = new HashSet<ObjectAddress>();
            _runningListeners = new Dictionary<int, LinkStreamListener>();
        }

        private static LinkStreamListener GetRunningListener(int port)
        {
            lock (_staticLock)
            {
                _runningListeners.TryGetValue(port, out var listener);
                return listener;
            }
        }

        private static void RegisterRunningListener(LinkStreamListener listener)
        {
            lock (_staticLock)
            {
                _runningListeners.Add(listener.Port, listener);
            }
        }

        public static void Connect(int port, Link link)
        {
            lock (_staticLock)
            {
                var address = new ObjectAddress(link);
                if (_usedLinks.Contains(address))
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                var listener = GetRunningListener(port);

                if (listener == null)
                {
                    throw new ApplicationException("Connection refused"); // todo2[ak] connection refused
                }

                _usedLinks.Add(new ObjectAddress(link));
                listener.QueueLinkForAcceptance(link);
            }
        }

        #endregion

        #region Fields

        private readonly object _lock;
        private bool _isDisposed;
        private readonly Queue<Link> _links;
        private readonly AutoResetEvent _signal;

        #endregion

        #region Constructor

        public LinkStreamListener(int port)
        {
            this.Port = port;
            _links = new Queue<Link>();
            _signal = new AutoResetEvent(false);
            _lock = new object();
        }

        #endregion

        #region Private

        private void QueueLinkForAcceptance(Link link)
        {
            lock (_lock)
            {
                _links.Enqueue(link);
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

        public string LocalEndpointName => $"link:{this.Port}";

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

                    if (_links.Count > 0)
                    {
                        var link = _links.Dequeue();
                        return link.Stream2;
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

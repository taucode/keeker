using Keeker.Core.Conf;
using Keeker.Core.Events;
using Keeker.Core.Listeners;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Keeker.Core.Proxies
{
    public class Proxy : IProxy
    {
        #region Fields

        private readonly ProxyPlainConf _conf;
        private readonly Dictionary<string, CertificateInfo> _certificates;
        private readonly List<IListener> _listeners;
        private readonly object _lock;
        private bool _isRunning;
        private bool _isDisposed;

        #endregion

        #region Constructor

        public Proxy(ProxyPlainConf conf)
        {
            _lock = new object();
            _conf = conf.Clone();

            _certificates = conf.Certificates
                .ToDictionary(
                    x => x.Key,
                    x => new CertificateInfo(
                            x.Value.Domains.ToArray(),
                            new X509Certificate2(
                                x.Value.FilePath,
                                x.Value.Password)));

            _listeners = conf.Listeners.Values
                .Select(x => (IListener)new Listener(
                    x,
                    x.GetUserCertificateIds()
                        .Select(y => _certificates[y])
                        .ToArray()))
                .ToList();

            foreach (var listener in _listeners)
            {
                listener.ConnectionAccepted += listener_ConnectionAccepted;
                listener.RelayStarted += listener_RelayStarted;
                listener.RelayDisposed += listener_RelayDisposed;
            }
        }

        #endregion

        #region Event Handlers

        private void listener_ConnectionAccepted(object sender, ConnectionAcceptedEventArgs e)
        {
            this.ConnectionAccepted?.Invoke(sender, e);
        }

        private void listener_RelayStarted(object sender, RelayEventArgs e)
        {
            this.RelayStarted?.Invoke(sender, e);
        }

        private void listener_RelayDisposed(object sender, RelayEventArgs e)
        {
            this.RelayDisposed?.Invoke(sender, e);
        }


        //private void listener_RelayCreated(object sender, RelayEventArgs e)
        //{
        //    this.ListenerRelayCreated?.Invoke(sender, e);
        //}

        #endregion

        #region IProxy Members

        public ProxyPlainConf GetConf() => _conf.Clone();

        public void Start()
        {
            lock (_lock)
            {
                if (_isRunning)
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                if (_isDisposed)
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                foreach (var listener in _listeners)
                {
                    listener.Start();
                }

                _isRunning = true;
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (!_isRunning)
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                if (_isDisposed)
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                foreach (var listener in _listeners)
                {
                    listener.Dispose();
                }

                _isRunning = false;
            }
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

        public IListener[] GetListeners()
        {
            lock (_lock)
            {
                return _listeners.ToArray();
            }
        }

        public StreamWriter Log { get; set; }

        public event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        public event EventHandler<RelayEventArgs> RelayStarted;

        public event EventHandler<RelayEventArgs> RelayDisposed;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (_lock)
            {
                if (_isDisposed)
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                if (_isRunning)
                {
                    this.Stop();
                }

                _isDisposed = true;
            }
        }

        #endregion
    }
}

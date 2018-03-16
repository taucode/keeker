using Keeker.Core.Conf;
using Keeker.Core.Events;
using Keeker.Core.Listeners;
using System;
using System.Collections.Generic;
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

        #endregion

        #region Constructor

        public Proxy(ProxyPlainConf conf)
        {
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
                listener.RelayCreated += listener_RelayCreated;
            }
        }

        #endregion

        #region Event Handlers

        private void listener_ConnectionAccepted(object sender, ConnectionAcceptedEventArgs e)
        {
            this.ListenerConnectionAccepted?.Invoke(sender, e);
        }

        private void listener_RelayCreated(object sender, RelayEventArgs e)
        {
            this.ListenerRelayCreated?.Invoke(sender, e);
        }

        #endregion

        #region IProxy Members

        public ProxyPlainConf GetConf() => _conf.Clone();

        public void Start()
        {
            foreach (var listener in _listeners)
            {
                listener.Start();
            }
        }

        public void Stop()
        {
            foreach (var listener in _listeners)
            {
                listener.Stop();
            }
        }

        public event EventHandler<ConnectionAcceptedEventArgs> ListenerConnectionAccepted;

        public event EventHandler<RelayEventArgs> ListenerRelayCreated;

        #endregion
    }
}

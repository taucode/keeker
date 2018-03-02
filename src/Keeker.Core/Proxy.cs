using Keeker.Core.Conf;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Keeker.Core
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
                            new X509Certificate(
                                x.Value.FilePath,
                                x.Value.Password)));

            _listeners = conf.Listeners.Values
                .Select(x => (IListener)new Listener(
                    x,
                    x.GetUserCertificateIds()
                        .Select(y => _certificates[y])
                        .ToArray()))
                .ToList();
        }

        #endregion

        #region Event Handlers

        //private void listener_Started(object sender, EventArgs e)
        //{
        //    this.Started?.Invoke(this, e);
        //}

        //private void listener_Stopped(object sender, EventArgs e)
        //{
        //    this.Stopped?.Invoke(this, e);
        //}

        //private void listener_ConnectionAccepted(object sender, ConnectionAcceptedEventArgs e)
        //{
        //    this.ConnectionAccepted?.Invoke(this, e);
        //    this.EstablishConnection(e.TcpClient);
        //}

        #endregion

        #region Private




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

        //public event EventHandler Started;

        //public event EventHandler Stopped;

        //public event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        #endregion
    }
}

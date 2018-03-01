using System;
using System.Net;

namespace Keeker.Core
{
    public class Proxy : IProxy
    {
        #region Constants

        private const int HANDSHAKE_MESSAGE_MAX_LENGTH = 1000;

        #endregion

        #region Fields

        private readonly ProxyPlainConf _conf;
        private readonly IListener _listener;
        

        #endregion

        #region Constructor

        public Proxy(ProxyPlainConf conf)
        {
            _conf = conf.Clone();
            _listener = new Listener(new IPEndPoint(_conf.Address, _conf.Port));

            _listener.Started += listener_Started;
            _listener.Stopped += listener_Stopped;
            _listener.ConnectionAccepted += listener_ConnectionAccepted;
            _targets = this.BuildTargets();
        }

        #endregion

        #region Event Handlers

        private void listener_Started(object sender, EventArgs e)
        {
            this.Started?.Invoke(this, e);
        }

        private void listener_Stopped(object sender, EventArgs e)
        {
            this.Stopped?.Invoke(this, e);
        }

        private void listener_ConnectionAccepted(object sender, ConnectionAcceptedEventArgs e)
        {
            this.ConnectionAccepted?.Invoke(this, e);
            this.EstablishConnection(e.TcpClient);
        }

        #endregion

        #region Private

        


        #endregion

        #region IProxy Members

        public ProxyPlainConf GetConf() => _conf.Clone();

        public void Start()
        {
            _listener.Start();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        public event EventHandler Started;

        public event EventHandler Stopped;

        public event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        #endregion
    }
}

using Keeker.Core.Conf;
using System.Collections.Generic;

namespace Keeker.Core
{
    public class Proxy : IProxy
    {
        #region Fields

        private readonly ProxyPlainConf _conf;
        //private readonly IListener _listener;
        private readonly List<IListener> _listeners;

        
        #endregion

        #region Constructor

        public Proxy(ProxyPlainConf conf)
        {
            _conf = conf.Clone();
            _listeners = new List<IListener>();

            foreach (var listenerConf in conf.Listeners.Values)
            {
                var listener = new Listener(listenerConf);
                _listeners.Add(listener);
            }

            //_listener = new Listener(new IPEndPoint(_conf.Address, _conf.Port));

            //_listener.Started += listener_Started;
            //_listener.Stopped += listener_Stopped;
            //_listener.ConnectionAccepted += listener_ConnectionAccepted;
            //_targets = this.BuildTargets();
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

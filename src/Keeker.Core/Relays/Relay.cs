using Keeker.Core.Conf;
using Keeker.Core.Listeners;
using Keeker.Core.Relays.StreamRedirectors;
using Keeker.Core.Streams;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Keeker.Core.Relays
{
    public class Relay : IRelay
    {
        #region Constants

        private const int DEFAULT_HTTPS_PORT = 443;
        private const int DEFAULT_HTTP_PORT = 80;

        #endregion

        #region Fields
        
        private readonly Listener _listener;

        private bool _isRunning;
        private readonly object _lock;

        private readonly bool _isHttps;

        private readonly KeekStream _clientStream;
        private readonly KeekStream _serverStream;

        private readonly string _domesticAuthority;
        private readonly string _domesticAuthorityWithPort;
        
        #endregion

        #region Constructor

        internal Relay(
            Listener listener,
            string id,
            bool isHttps,
            Stream innerClientStream,
            HostPlainConf conf)
        {
            _listener = listener;
            this.Id = id;

            if (conf.EndPoint.Port == DEFAULT_HTTPS_PORT)
            {
                throw new ApplicationException(); // todo2[ak] suspicious: why domestic has port equal to SSL? we do not authorize as client
            }

            _lock = new object();

            _isHttps = isHttps;

            this.ExternalHostName = conf.ExternalHostName;

            _clientStream = new KeekStream(innerClientStream, true);

            var tcpclient = new TcpClient();
            tcpclient.Connect(conf.EndPoint);
            var serverNetworkStream = tcpclient.GetStream();

            _serverStream = new KeekStream(serverNetworkStream, true);

            string colonWithPortIfNeeded;
            if (conf.EndPoint.Port == DEFAULT_HTTP_PORT)
            {
                colonWithPortIfNeeded = "";
            }
            else
            {
                colonWithPortIfNeeded = $":{conf.EndPoint.Port}";
            }

            _domesticAuthority = $"{conf.DomesticHostName}{colonWithPortIfNeeded}";
            _domesticAuthorityWithPort = $"{conf.DomesticHostName}:{conf.EndPoint.Port}";

            this.Signal = new ManualResetEvent(false);
        }

        #endregion

        #region Private

        private string GetProtocol()
        {
            return _isHttps ? "https" : "http";
        }

        #endregion

        #region Internal

        internal ManualResetEvent Signal { get; private set; }

        #endregion

        #region IRelay Members

        public string Id { get; }

        public string ListenerId => _listener.Id;

        public void Start()
        {
            lock (_lock)
            {
                if (_isRunning)
                {
                    throw new ApplicationException(); // todo2[ak]
                }
            }

            var clientRedirector = new ClientStreamRedirector(
                this,
                _clientStream,
                _serverStream,
                this.ExternalHostName,
                _domesticAuthority);

            var serverRedirector = new ServerStreamRedirector(
                this,
                _serverStream,
                _clientStream,
                this.GetProtocol(),
                this.ExternalHostName,
                _domesticAuthority,
                _domesticAuthorityWithPort);

            clientRedirector.Start();
            serverRedirector.Start();

            lock (_lock)
            {
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
            }

            throw new NotImplementedException();
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

        public string ExternalHostName { get; }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // don't forget signal!!
            throw new NotImplementedException();
        }

        #endregion
    }
}

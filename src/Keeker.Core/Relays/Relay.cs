using Keeker.Core.Conf;
using Keeker.Core.Relays.StreamRedirectors;
using Keeker.Core.Streams;
using System;
using System.IO;
using System.Net.Sockets;

namespace Keeker.Core.Relays
{
    public class Relay
    {
        private const int DEFAULT_HTTPS_PORT = 443;
        private const int DEFAULT_HTTP_PORT = 80;

        private bool _isStarted;
        private readonly object _lock;

        private readonly bool _isHttps;

        private readonly KeekStream _clientStream;
        private readonly KeekStream _serverStream;

        private readonly string _domesticAuthority;
        private readonly string _domesticAuthorityWithPort;

        public Relay(
            bool isHttps,
            Stream innerClientStream,
            string listenerId,
            string id,
            HostPlainConf conf)
        {
            if (conf.EndPoint.Port == DEFAULT_HTTPS_PORT)
            {
                throw new ApplicationException(); // todo2[ak] suspicious: why domestic has port equal to SSL? we do not authorize as client
            }

            _lock = new object();

            _isHttps = isHttps;

            this.ExternalHostName = conf.ExternalHostName;

            _clientStream = new KeekStream(innerClientStream);

            var tcpclient = new TcpClient();
            tcpclient.Connect(conf.EndPoint);
            var serverNetworkStream = tcpclient.GetStream();

            _serverStream = new KeekStream(serverNetworkStream);

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
        }
        
        public string ExternalHostName { get; }

        public void Start()
        {
            lock (_lock)
            {
                if (_isStarted)
                {
                    throw new ApplicationException(); // todo2[ak]
                }
            }

            var clientRedirector = new ClientStreamRedirector(
                _clientStream,
                _serverStream,
                this.ExternalHostName,
                _domesticAuthority);

            var serverRedirector = new ServerStreamRedirector(
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
                _isStarted = true;
            }
        }

        private string GetProtocol()
        {
            return _isHttps ? "https" : "http";
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (!_isStarted)
                {
                    throw new ApplicationException(); // todo2[ak]
                }
            }

            throw new NotImplementedException();
        }

        public bool IsStarted
        {
            get
            {
                lock (_lock)
                {
                    return _isStarted;
                }
            }
        }
    }
}

using Keeker.Core.Conf;
using Keeker.Core.EventData;
using Keeker.Core.Relays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Keeker.Core
{
    public class Listener : IListener
    {
        #region Constants

        private const int HANDSHAKE_MESSAGE_MAX_LENGTH = 1000;

        #endregion

        #region Fields

        private readonly ListenerPlainConf _conf;
        private readonly TcpListener _tcpListener;
        private bool _isStarted;
        private readonly object _lock;

        private int _nextRelayId;
        private readonly object _nextRelayIdLock;

        private readonly Dictionary<string, X509Certificate2> _certificates;
        private readonly Dictionary<string, byte[]> _binaryDomainNames;

        #endregion

        #region Constructor

        public Listener(ListenerPlainConf conf, CertificateInfo[] certificates)
        {
            this.Id = conf.Id;
            _nextRelayIdLock = new object();

            _conf = conf.Clone();
            _certificates = new Dictionary<string, X509Certificate2>();

            foreach (var certificateInfo in certificates)
            {
                foreach (var domainName in certificateInfo.DomainNames)
                {
                    _certificates.Add(domainName, certificateInfo.Certificate);
                }
            }

            _binaryDomainNames = _conf.Hosts.Values
                .ToDictionary(
                    x => x.ExternalHostName,
                    x => x.ExternalHostName.ToAsciiBytes());

            _tcpListener = new TcpListener(_conf.EndPoint);
            _lock = new object();
        }

        #endregion

        #region Private

        private static X509Certificate2 OpenCertificate(CertificatePlainConf certificateConf)
        {
            var cert = new X509Certificate2(certificateConf.FilePath, certificateConf.Password);
            return cert;
        }

        private void StartImpl()
        {
            var task = new Task(this.ListeningRoutine);
            _isStarted = true;
            task.Start();
            //this.Started?.Invoke(this, EventArgs.Empty);
        }

        private void ListeningRoutine()
        {
            _tcpListener.Start();

            try
            {
                while (true)
                {
                    var client = _tcpListener.AcceptTcpClient();

                    this.EstablishConnection(client);

                    this.ConnectionAccepted?.Invoke(this, new ConnectionAcceptedEventArgs(client));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void EstablishConnection(TcpClient client)
        {
            if (_conf.IsHttps)
            {
                var networkStream = client.GetStream();
                var wrappingStream = new KeekStream(networkStream);

                var hostConf = this.ResolveHostConf(wrappingStream);

                if (hostConf == null)
                {
                    wrappingStream.Dispose(); // refuse the connection.
                    return;
                }

                var clientStream = new SslStream(wrappingStream, false);
                var certificate = _certificates[hostConf.ExternalHostName];
                clientStream.AuthenticateAsServer(certificate, false, SslProtocols.Tls12, false);

                var relayId = this.GetNextRelayId();

                var relay = new Relay(clientStream, _conf.Id, relayId, hostConf);
                this.RelayCreated?.Invoke(this, new RelayEventArgs(relay));
                relay.Start();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private string GetNextRelayId()
        {
            lock (_nextRelayIdLock)
            {
                _nextRelayId++;
                return $"{this.Id}.{_nextRelayId}";
            }
        }

        private HostPlainConf ResolveHostConf(KeekStream keekStream)
        {
            const int TIMEOUT = 1; // we are waiting for incoming handshake for 1 second.
            var timeout = TimeSpan.FromSeconds(TIMEOUT);

            var started = DateTime.UtcNow;

            var peekedBytes = new byte[HANDSHAKE_MESSAGE_MAX_LENGTH];

            while (true)
            {
                keekStream.ReadInnerStream(HANDSHAKE_MESSAGE_MAX_LENGTH);
                var peekedBytesCount = keekStream.Peek(peekedBytes, 0, HANDSHAKE_MESSAGE_MAX_LENGTH);

                foreach (var hostConf in _conf.Hosts.Values)
                {
                    var binaryDomainName = _binaryDomainNames[hostConf.ExternalHostName];
                    var pos = peekedBytes.IndexOfSubarray(binaryDomainName, 0, peekedBytesCount);

                    if (pos >= 0)
                    {
                        return hostConf;
                    }
                }

                Thread.Sleep(1); // wait for a while, maybe will get handshake eventually
                var now = DateTime.UtcNow;

                if (now - started > timeout)
                {
                    return null;
                }
            }
        }

        #endregion

        #region IListener Members

        public string Id { get; }

        public void Start()
        {
            lock (_lock)
            {
                if (_isStarted)
                {
                    throw new InvalidOperationException("Listener already started");
                }

                this.StartImpl();
            }
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

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        public event EventHandler<RelayEventArgs> RelayCreated;

        //public IPEndPoint EndPoint
        //{
        //    get
        //    {
        //        lock (_lock)
        //        {
        //            return _conf.GetEndPoint();
        //        }
        //    }
        //}

        //public event EventHandler Started;

        //public event EventHandler Stopped;

        //public event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
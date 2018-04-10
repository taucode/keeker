using Keeker.Convey.Conf;
using Keeker.Convey.Logging;
using Keeker.Convey.Relays;
using Keeker.Convey.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Keeker.Convey.Listeners
{
    public class Listener : IListener
    {
        #region Constants

        private const int HANDSHAKE_MESSAGE_MAX_LENGTH = 1000;

        #endregion

        #region Logging

        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        #endregion

        #region Fields

        private readonly ListenerPlainConf _conf;
        private bool _isRunning;
        private bool _isDisposed;
        private readonly object _lock;
        private readonly Task _listeningTask;
        private TcpListener _tcpListener;

        private readonly Dictionary<string, X509Certificate2> _certificates;
        private readonly Dictionary<string, byte[]> _binaryDomainNames;

        private int _nextRelayId;

        #endregion

        #region Constructor

        public Listener(ListenerPlainConf conf, CertificateInfo[] certificates)
        {
            _conf = conf.Clone();
            _lock = new object();
            _listeningTask = new Task(this.ListeningRoutine);

            _certificates = new Dictionary<string, X509Certificate2>();

            foreach (var certificateInfo in certificates)
            {
                foreach (var domainName in certificateInfo.DomainNames)
                {
                    _certificates.Add(domainName, certificateInfo.Certificate);
                }
            }

            _binaryDomainNames = _conf.Hosts
                .ToDictionary(
                    x => x.ExternalHostName,
                    x => x.ExternalHostName.ToAsciiBytes());
        }

        #endregion

        #region Private

        private void ListeningRoutine()
        {
            lock (_lock)
            {
                try
                {
                    _tcpListener = new TcpListener(_conf.IPEndPoint);
                    _tcpListener.Start();

                    Logger.InfoFormat("Started TCP listener (Id: {0}) at {1}", _conf.Id, _conf.IPEndPoint);
                }
                catch (Exception ex)
                {
                    Logger.ErrorException("Could not start TCP listener", ex);

                    try
                    {
                        _tcpListener.Stop();
                    }
                    catch
                    {
                        // dismiss
                    }

                    _isRunning = false;

                    // no re-throw
                    return;
                }
            }

            TcpClient client = null;
            Relay relay = null;

            try
            {
                while (true)
                {
                    client = _tcpListener.AcceptTcpClient();

                    Logger.InfoFormat("Listener {0} accepted client: {1}", _conf.Id, client.Client.RemoteEndPoint);

                    try
                    {
                        relay = this.EstablishConnection(client);
                        relay.Start();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            client?.Dispose();
                        }
                        catch
                        {
                            // dismiss
                        }

                        try
                        {
                            relay?.Dispose();
                        }
                        catch
                        {
                            // dismiss
                        }

                        Logger.ErrorException("Could not start relay", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(); // todo000000[ak]
            }
        }

        private string GetNextRelayId(string externalHostName)
        {
            var incremented = Interlocked.Increment(ref _nextRelayId);
            return $"{_conf.Id};{externalHostName};{incremented}";
        }

        private Relay EstablishConnection(TcpClient client)
        {
            if (_conf.IsHttps)
            {
                var networkStream = client.GetStream();
                var wrappingStream = new KeekStream(networkStream, false);

                var hostConf = this.ResolveHostConf(wrappingStream);

                if (hostConf == null)  // todo0000000[ak] hit into this situation!
                {
                    wrappingStream.Dispose(); // refuse the connection.
                    return null;
                }

                var clientStream = new SslStream(wrappingStream, false);
                var certificate = _certificates[hostConf.ExternalHostName];
                clientStream.AuthenticateAsServer(certificate, false, SslProtocols.Tls12, false);

                var relayId = this.GetNextRelayId(hostConf.ExternalHostName);

                var serverStream = this.CreateServerStream(hostConf);

                string colonWithPortIfNeeded;
                if (hostConf.EndPoint.Port == Helper.DEFAULT_HTTP_PORT)
                {
                    colonWithPortIfNeeded = "";
                }
                else
                {
                    colonWithPortIfNeeded = $":{hostConf.EndPoint.Port}";
                }

                var domesticAuthority = $"{hostConf.DomesticHostName}{colonWithPortIfNeeded}";
                var domesticAuthorityWithPort = $"{hostConf.DomesticHostName}:{hostConf.EndPoint.Port}";

                var relay = new Relay(
                    relayId,
                    clientStream,
                    serverStream,
                    hostConf.ExternalHostName,
                    domesticAuthority,
                    domesticAuthorityWithPort,
                    _conf.IsHttps);

                return relay;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private Stream CreateServerStream(HostPlainConf hostConf)
        {
            var tcpclient = new TcpClient();
            tcpclient.Connect(hostConf.EndPoint);

            var socket = tcpclient.Client;

            Logger.InfoFormat("Allocated socket: handle={0}, local={1}, remote={2}", socket.Handle, socket.LocalEndPoint, socket.RemoteEndPoint);

            var stream = tcpclient.GetStream();
            return stream;
        }

        private HostPlainConf ResolveHostConf(KeekStream keekStream)
        {
            const int TIMEOUT = 1; // we are waiting for incoming handshake for 1 second.
            var timeout = TimeSpan.FromSeconds(TIMEOUT);

            var startTime = DateTime.UtcNow;

            var peekedBytes = new byte[HANDSHAKE_MESSAGE_MAX_LENGTH];

            while (true)
            {
                keekStream.ReadInnerStream(HANDSHAKE_MESSAGE_MAX_LENGTH);
                var peekedBytesCount = keekStream.Peek(peekedBytes, 0, HANDSHAKE_MESSAGE_MAX_LENGTH);

                foreach (var hostConf in _conf.Hosts)
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

                if (now - startTime > timeout)
                {
                    return null;
                }
            }
        }

        #endregion

        #region IListener Members

        public void Start()
        {
            lock (_lock)
            {
                try
                {
                    if (_isRunning)
                    {
                        throw new InvalidOperationException("Listener already running");
                    }

                    if (_isDisposed)
                    {
                        throw new ObjectDisposedException("Listener");
                    }

                    _isRunning = true;
                    _listeningTask.Start();
                }
                catch (Exception ex)
                {
                    Logger.ErrorException("Error occured while starting listener", ex);
                    throw;
                }
            }
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
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

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}

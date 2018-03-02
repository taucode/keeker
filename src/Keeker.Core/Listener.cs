using Keeker.Core.Conf;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
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

        //private readonly Dictionary<string, byte[]> _hostNameBytes;
        private Dictionary<string, X509Certificate> _certificates;

        #endregion

        #region Constructor

        public Listener(ListenerPlainConf conf)
        {
            _conf = conf.Clone();

            //_hostNameBytes = _conf.Hosts.Values
            //    .ToDictionary(
            //        x => x.ExternalHostName,
            //        x => x.ExternalHostName.ToAsciiBytes());

            _tcpListener = new TcpListener(_conf.EndPoint);
            _lock = new object();
        }

        #endregion

        #region Private

        private static X509Certificate OpenCertificate(CertificatePlainConf certificateConf)
        {
            var cert = new X509Certificate(certificateConf.FilePath, certificateConf.Password);
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

                    //this.ConnectionAccepted?.Invoke(this, new ConnectionAcceptedEventArgs(client));
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

                throw new NotImplementedException();
                //var hostConf = this.ResolveHost(wrappingStream);

                //if (hostConf == null)
                //{
                //    wrappingStream.Dispose(); // refuse the connection.
                //    return;
                //}

                //var clientStream = new SslStream(wrappingStream, false);
                //var certificate = _certificates[hostConf.ExternalHostName];
                //clientStream.AuthenticateAsServer(certificate, false, SslProtocols.Tls12, false);

                //var relay = new SecureRelay(clientStream, hostConf.Relay);
                //relay.Start();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        //private HostPlainConf ResolveHost(KeekStream keekStream)
        //{
        //    const int TIMEOUT = 1; // we are waiting for incoming handshake for 1 second.
        //    var timeout = TimeSpan.FromSeconds(TIMEOUT);

        //    var started = DateTime.UtcNow;

        //    var peekedBytes = new byte[HANDSHAKE_MESSAGE_MAX_LENGTH];

        //    while (true)
        //    {
        //        keekStream.ReadInnerStream(HANDSHAKE_MESSAGE_MAX_LENGTH);
        //        var peekedBytesCount = keekStream.Peek(peekedBytes, 0, HANDSHAKE_MESSAGE_MAX_LENGTH);

        //        foreach (var hostConf in _conf.Hosts.Values)
        //        {
        //            var hostNameBytes = _hostNameBytes[hostConf.ExternalHostName];
        //            var pos = peekedBytes.IndexOfSubarray(hostNameBytes, 0, peekedBytesCount);

        //            if (pos >= 0)
        //            {
        //                return hostConf;
        //            }
        //        }

        //        Thread.Sleep(1); // wait for a while, maybe will get handshake eventually
        //        var now = DateTime.UtcNow;

        //        if (now - started > timeout)
        //        {
        //            return null;
        //        }
        //    }
        //}

        #endregion

        #region IListener Members

        public void Start()
        {
            lock (_lock)
            {
                throw new NotImplementedException();
                //if (_conf.IsHttps && _certificates == null)
                //{
                //    _certificates = _conf.Hosts
                //        .ToDictionary(
                //            x => x.Key,
                //            x => OpenCertificate(x.Value.Certificate));
                //}

                //if (_isStarted)
                //{
                //    throw new InvalidOperationException("Listener already started");
                //}

                //this.StartImpl();
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Keeker.Core
{
    public class Listener : IListener
    {
        #region Nested

        private class Target
        {
            internal Target(HostPlainConf hostEntry)
            {
                this.ExternalHostName = hostEntry.ExternalHostName;
                this.ExternalHostNameBytes = this.ExternalHostName.ToAsciiBytes();

                var entryTarget = hostEntry.Targets.Single(x => x.IsActive);

                this.DomesticHostName = entryTarget.DomesticHostName;
                this.Certificate = new X509Certificate(hostEntry.Certificate.FilePath, hostEntry.Certificate.Password);
                this.EndPoint = new IPEndPoint(entryTarget.Address, entryTarget.Port);
            }

            internal string ExternalHostName { get; }
            internal byte[] ExternalHostNameBytes { get; }
            internal string DomesticHostName { get; }
            internal X509Certificate Certificate { get; }
            internal IPEndPoint EndPoint { get; }
        }

        #endregion

        #region Fields

        private readonly IPEndPoint _endPoint;
        private readonly TcpListener _listener;
        private bool _isStarted;
        private readonly object _lock;
        private readonly Dictionary<string, Target> _targets;

        #endregion

        #region Constructor

        public Listener(IPEndPoint endPoint)
        {
            _endPoint = endPoint.CloneEndPoint();
            _listener = new TcpListener(_endPoint);
            _lock = new object();
        }

        #endregion

        #region Private

        private void StartImpl()
        {
            var task = new Task(this.ListeningRoutine);
            _isStarted = true;
            task.Start();
            this.Started?.Invoke(this, EventArgs.Empty);
        }

        private void ListeningRoutine()
        {
            _listener.Start();

            try
            {
                while (true)
                {
                    var client = _listener.AcceptTcpClient();
                    this.ConnectionAccepted?.Invoke(this, new ConnectionAcceptedEventArgs(client));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private Dictionary<string, Target> BuildTargets()
        {
            var result = _conf.Hosts.Values
                .Select(x => new Target(x))
                .ToDictionary(x => x.ExternalHostName, x => x);

            return result;
        }

        private void EstablishConnection(TcpClient client)
        {
            var networkStream = client.GetStream();

            var wrappingStream = new KeekStream(networkStream);
            var target = this.ResolveHost(wrappingStream);

            if (target == null)
            {
                // could not resolve.

                try
                {
                    wrappingStream.Dispose();
                }
                catch
                {
                    // dismiss
                }

                return;
            }

            var clientStream = new SslStream(wrappingStream, false);
            clientStream.AuthenticateAsServer(target.Certificate, false, SslProtocols.Tls12, false);

            var tcpclient = new TcpClient();
            tcpclient.Connect(target.EndPoint);

            var serverStream = tcpclient.GetStream();

            var connection = new WebConnection(clientStream, serverStream);
            connection.Start();
        }

        private Target ResolveHost(KeekStream keekStream)
        {
            const int TIMEOUT = 1; // we are waiting for incoming handshake for 1 second.
            var timeout = TimeSpan.FromSeconds(TIMEOUT);

            var started = DateTime.UtcNow;

            var peekedBytes = new byte[HANDSHAKE_MESSAGE_MAX_LENGTH];

            while (true)
            {
                keekStream.ReadInnerStream(HANDSHAKE_MESSAGE_MAX_LENGTH);
                var peekedBytesCount = keekStream.Peek(peekedBytes, 0, HANDSHAKE_MESSAGE_MAX_LENGTH);

                foreach (var target in _targets.Values)
                {
                    var hostNameBytes = target.ExternalHostNameBytes;
                    var pos = peekedBytes.IndexOfSubarray(hostNameBytes, 0, HANDSHAKE_MESSAGE_MAX_LENGTH);

                    if (pos >= 0)
                    {
                        return target;
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

        public IPEndPoint EndPoint => _endPoint.CloneEndPoint();

        public event EventHandler Started;

        public event EventHandler Stopped;

        public event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion
    }
}

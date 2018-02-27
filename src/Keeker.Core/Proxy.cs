using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Keeker.Core
{
    public class Proxy : IProxy
    {
        #region Nested

        private class Target
        {
            internal Target(ProxyPlainConf.HostEntry hostEntry)
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

        #region Constants

        private const int HANDSHAKE_MESSAGE_MAX_LENGTH = 1000;

        #endregion

        #region Fields

        private readonly ProxyPlainConf _conf;
        private readonly IListener _listener;
        private readonly Dictionary<string, Target> _targets;

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
                    client.Dispose();
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

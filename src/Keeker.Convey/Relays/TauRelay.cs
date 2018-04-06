using Keeker.Convey.Logging;
using Keeker.Convey.Relays.StreamRedirectors;
using Keeker.Convey.Streams;
using System;
using System.IO;
using System.Threading;

namespace Keeker.Convey.Relays
{
    public class TauRelay : ITauRelay
    {
        #region Logging

        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        #endregion

        #region Fields

        private bool _isRunning;
        private bool _isDisposed;
        private readonly object _lock;

        private readonly KeekStream _clientStream;
        private readonly KeekStream _serverStream;
        private readonly string _externalHostName;
        private readonly string _domesticAuthority;
        private readonly string _domesticAuthorityWithPort;
        private readonly string _protocol;
        private readonly ManualResetEvent _stopSignal;

        #endregion

        #region Constructor

        public TauRelay(
            Stream innerClientStream,
            Stream innerServerStream,
            string externalHostName,
            string domesticAuthority,
            string domesticAuthorityWithPort,
            bool isHttps)
        {
            _lock = new object();
            _clientStream = new KeekStream(innerClientStream);
            _serverStream = new KeekStream(innerServerStream);
            _externalHostName = externalHostName;
            _domesticAuthority = domesticAuthority;
            _domesticAuthorityWithPort = domesticAuthorityWithPort;
            _protocol = isHttps ? "https" : "http";
            _stopSignal = new ManualResetEvent(false);
        }

        #endregion

        #region ITauRelay Members

        public void Start()
        {
            lock (_lock)
            {
                try
                {
                    if (_isRunning)
                    {
                        throw new ApplicationException();
                    }

                    if (_isDisposed)
                    {
                        throw new NotImplementedException();
                    }

                    _isRunning = true;

                    var clientRedirector = new ClientStreamRedirector(
                        _stopSignal,
                        _clientStream,
                        _serverStream,
                        _externalHostName,
                        _domesticAuthority);

                    var serverRedirector = new ServerStreamRedirector(
                        _stopSignal,
                        _serverStream,
                        _clientStream,
                        _protocol,
                        _externalHostName,
                        _domesticAuthority,
                        _domesticAuthorityWithPort);

                    clientRedirector.Start();
                    serverRedirector.Start();
                }
                catch (Exception ex)
                {
                    Logger.ErrorException("Error occured while starting relay", ex);
                    throw;
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new System.NotImplementedException(); // todo000000[ak] stop signal!
        }

        #endregion
    }
}

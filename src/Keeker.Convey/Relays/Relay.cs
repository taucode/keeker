using Keeker.Convey.Logging;
using Keeker.Convey.Relays.StreamRedirectors;
using System;
using System.IO;
using System.Threading;
using Keeker.Core.Streams;

namespace Keeker.Convey.Relays
{
    public class Relay : IDisposable
    {
        #region Logging

        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        #endregion

        #region Fields

        private readonly string _id;

        private bool _isRunning;
        private bool _isDisposed;
        private readonly object _lock;

        private readonly KeekStream _clientStream;
        private readonly ClientStreamRedirector _clientStreamRedirector;

        private readonly KeekStream _serverStream;
        private readonly ServerStreamRedirector _serverStreamRedirector;

        private readonly ManualResetEvent _stopSignal;

        #endregion

        #region Constructor

        public Relay(
            string id,
            Stream innerClientStream,
            Stream innerServerStream,
            string externalHostName,
            string domesticAuthority,
            string domesticAuthorityWithPort,
            bool isHttps)
        {
            _id = id;
            _lock = new object();

            _clientStream = new KeekStream(innerClientStream, false);

            _serverStream = new KeekStream(innerServerStream, false);

            var externalHostName1 = externalHostName;
            var domesticAuthority1 = domesticAuthority;
            var protocol = isHttps ? "https" : "http";
            _stopSignal = new ManualResetEvent(false);

            _clientStreamRedirector = new ClientStreamRedirector(
                this,
                _clientStream,
                _serverStream,
                externalHostName1,
                domesticAuthority1);

            _serverStreamRedirector = new ServerStreamRedirector(
                this,
                _serverStream,
                _clientStream,
                protocol,
                externalHostName1,
                domesticAuthority1,
                domesticAuthorityWithPort);
        }

        #endregion

        #region Private

        private void ExecuteIgnoringExceptions(Action action)
        {
            try
            {
                action();
            }
            catch
            {
                // dismiss
            }
        }

        #endregion

        #region Public

        public void Start()
        {
            lock (_lock)
            {
                try
                {
                    if (_isRunning)
                    {
                        throw new InvalidOperationException("Relay already running");
                    }

                    if (_isDisposed)
                    {
                        throw new ObjectDisposedException("Relay");
                    }

                    _isRunning = true;


                    _clientStreamRedirector.Start();
                    _serverStreamRedirector.Start();

                    Logger.InfoFormat("Relay {0} started", _id);
                }
                catch (Exception ex)
                {
                    Logger.ErrorException("Error occured while starting relay", ex);
                    throw;
                }
            }
        }

        public ManualResetEvent StopSignal => _stopSignal;

        public string Id => _id;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (_lock)
            {
                if (_isDisposed)
                {
                    return;
                }

                _isDisposed = true;
            }

            this.ExecuteIgnoringExceptions(() => _stopSignal.Set());

            this.ExecuteIgnoringExceptions(() => _clientStream.Dispose());
            this.ExecuteIgnoringExceptions(() => _serverStream.Dispose());

            _clientStreamRedirector.Wait();
            _serverStreamRedirector.Wait();

            this.ExecuteIgnoringExceptions(() => _stopSignal.Dispose());
        }

        #endregion
    }
}

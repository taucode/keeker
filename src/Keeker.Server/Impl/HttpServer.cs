using Keeker.Core;
using Keeker.Core.Listeners;
using Keeker.Server.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keeker.Server.Impl
{
    public class HttpServer : IHttpServer
    {
        #region Logging

        private static ILog GetLogger() => LogProvider.GetCurrentClassLogger();

        #endregion

        #region Fields

        private bool _isRunning;
        private bool _isDisposed;
        private readonly object _lock;
        private Task _listeningTask;
        private readonly IStreamListener _streamListener;
        private readonly IdGenerator _idGenerator;
        private readonly IHandlerFactory _handlerFactory;
        private readonly string[] _hosts;
        private readonly Dictionary<string, ServerConnection> _connections;

        #endregion

        #region Constructor

        public HttpServer(
            IStreamListener streamListener,
            string[] hosts,
            IHandlerFactory handlerFactory)
        {
            if (streamListener == null)
            {
                throw new ArgumentNullException(nameof(streamListener));
            }

            if (hosts == null)
            {
                throw new ArgumentNullException(nameof(hosts));
            }

            if (!hosts.Any())
            {
                throw new ArgumentException("At least one host is needed", nameof(hosts));
            }

            if (handlerFactory == null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            _lock = new object();
            _streamListener = streamListener;
            _hosts = hosts;
            _idGenerator = new IdGenerator();
            _connections = new Dictionary<string, ServerConnection>();
            _handlerFactory = handlerFactory;
        }

        #endregion

        #region Private

        private void ListeningRoutine()
        {
            lock (_lock)
            {
                try
                {
                    //_streamListener = this.CreateStreamListener();
                    _streamListener.Start();

                    GetLogger().InfoFormat("Listener started at {0}", _streamListener.LocalEndpointName);
                }
                catch (Exception ex)
                {
                    GetLogger().ErrorException("Could not start listener", ex);

                    try
                    {
                        _streamListener.Dispose();
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

            try
            {
                while (true)
                {
                    var stream = _streamListener.AcceptStream();

                    try
                    {
                        var id = _idGenerator.Generate();
                        var connection = new ServerConnection(this, id, stream, _handlerFactory);

                        lock (_lock)
                        {
                            _connections.Add(connection.Id, connection);
                        }

                        this.ConnectionAccepted?.Invoke(this, connection);

                        connection.Start();
                    }
                    catch (Exception ex)
                    {
                        throw new NotImplementedException(); // todo00000000000[ak]
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(); // todo00000000000[ak]
            }
        }

        #endregion

        #region IHttpServer Members

        public IStreamListener StreamListener => _streamListener;

        public void Start()
        {
            lock (_lock)
            {
                try
                {
                    if (_isRunning)
                    {
                        throw new InvalidOperationException("Server already running");
                    }

                    if (_isDisposed)
                    {
                        throw new ObjectDisposedException("Server");
                    }

                    _isRunning = true;

                    GetLogger().InfoFormat("Starting the server");

                    _listeningTask = new Task(this.ListeningRoutine);
                    _listeningTask.Start();

                }
                catch (Exception ex)
                {
                    GetLogger().ErrorException("Error occured while starting server", ex);
                    throw;
                }
            }
        }

        public string[] Hosts => _hosts.ToArray();

        public bool IsRunning => throw new NotImplementedException();

        public event EventHandler<ServerConnection> ConnectionAccepted;

        public event Action<string, byte[]> RawDataReceived;

        public event Action<string, byte[]> RawDataSent;

        public bool IsDisposed => throw new NotImplementedException();

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

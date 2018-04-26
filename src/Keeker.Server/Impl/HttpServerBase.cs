using Keeker.Core;
using Keeker.Core.Listeners;
using Keeker.Server.Logging;
using System;
using System.Threading.Tasks;

namespace Keeker.Server.Impl
{
    public abstract class HttpServerBase : IHttpServer
    {
        #region Logging

        private static ILog GetLogger() => LogProvider.GetCurrentClassLogger();

        #endregion

        #region Fields

        private bool _isRunning;
        private bool _isDisposed;
        private readonly object _lock;
        private Task _listeningTask;
        private IStreamListener _streamListener;
        private readonly IdGenerator _idGenerator;
        private readonly IHandlerFactory _handlerFactory;

        #endregion

        #region Constructor

        protected HttpServerBase()
        {
            _lock = new object();
            _idGenerator = new IdGenerator();
        }

        #endregion

        #region IHttpServer Members

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

        private void ListeningRoutine()
        {
            lock (_lock)
            {
                try
                {
                    _streamListener = this.CreateStreamListener();
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
                        var connection = new Connection(id, stream, _handlerFactory);

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


            //Socket clientSocket;

            //throw new NotImplementedException(); // todo00000000[ak]

            //TcpClient client = null;
            //Relay relay = null;

            //try
            //{
            //    while (true)
            //    {
            //        client = _tcpListener.AcceptTcpClient();

            //        Logger.InfoFormat("Listener {0} accepted client: {1}", _conf.Id, client.Client.RemoteEndPoint);

            //        try
            //        {
            //            relay = this.EstablishConnection(client);
            //            relay.Start();
            //        }
            //        catch (Exception ex)
            //        {
            //            try
            //            {
            //                client?.Dispose();
            //            }
            //            catch
            //            {
            //                // dismiss
            //            }

            //            try
            //            {
            //                relay?.Dispose();
            //            }
            //            catch
            //            {
            //                // dismiss
            //            }

            //            Logger.ErrorException("Could not start relay", ex);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new NotImplementedException(); // todo000000[ak]
            //}
        }

        protected abstract IStreamListener CreateStreamListener();

        public bool IsRunning { get; }

        public bool IsDisposed { get; }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

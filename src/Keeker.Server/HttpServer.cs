using Keeker.Core;
using Keeker.Server.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Keeker.Server
{
    public class HttpServer : IHttpServer
    {
        #region Logging

        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        #endregion

        #region Fields

        private bool _isRunning;
        private bool _isDisposed;
        private readonly object _lock;
        private readonly IPEndPoint _endPoint;
        private TcpListener _tcpListener;
        private readonly Task _listeningTask;

        private readonly IdGenerator _idGenerator;
        private readonly IHandlerFactory _handlerFactory;

        #endregion

        #region Constructor

        public HttpServer(IPEndPoint endPoint, IHandlerFactory handlerFactory)
        {
            _lock = new object();
            _endPoint = new IPEndPoint(endPoint.Address, endPoint.Port);
            _handlerFactory = handlerFactory;
            _listeningTask = new Task(this.ListeningRoutine);

            _idGenerator = new IdGenerator();
        }

        #endregion

        #region Private

        private void ListeningRoutine()
        {
            lock (_lock)
            {
                try
                {
                    _tcpListener = new TcpListener(_endPoint);
                    _tcpListener.Start();

                    Logger.InfoFormat("Server started at {0}", _endPoint);
                    Logger.InfoFormat("Server listening socket: {0}", _tcpListener.Server.ToInfoString());
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

            try
            {
                while (true)
                {
                    var clientSocket = _tcpListener.AcceptSocket();
                    Logger.InfoFormat("Server accepted connection: {0}", clientSocket.ToInfoString(true));

                    try
                    {
                        var id = _idGenerator.Generate();
                        var stream = new NetworkStream(clientSocket);
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

                    Logger.InfoFormat("Starting the server");

                    _listeningTask.Start();

                }
                catch (Exception ex)
                {
                    Logger.ErrorException("Error occured while starting server", ex);
                    throw;
                }
            }
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

        public bool IsDisposed
        {
            get
            {
                lock (_lock)
                {
                    return _isDisposed;
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (_lock)
            {
                if (_isDisposed)
                {
                    Logger.Info("Server already disposed");
                    return;
                }

                try
                {
                    _tcpListener.Stop();
                }
                catch
                {
                    // dismiss
                }

                try
                {
                    _tcpListener.Server.Dispose();
                }
                catch
                {
                    // dismiss
                }

                _isRunning = false;
                _isDisposed = true;
            }
        }

        #endregion
    }
}

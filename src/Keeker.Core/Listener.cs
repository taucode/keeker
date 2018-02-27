using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Keeker.Core
{
    public class Listener : IListener
    {
        #region Fields

        private readonly IPEndPoint _endPoint;
        private readonly TcpListener _listener;
        private bool _isStarted;
        private readonly object _lock;

        #endregion

        #region Constructor

        public Listener(IPEndPoint endPoint)
        {
            _endPoint = endPoint.ClonEndPoint();
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

        #endregion

        #region IKeekerListener Members

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

        public IPEndPoint EndPoint => _endPoint.ClonEndPoint();

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

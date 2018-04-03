using System;

namespace Keeker.Convey.Proxies
{
    public class TauProxy : ITauProxy
    {
        #region Fields

        private bool _isRunning;
        private bool _isDisposed;
        private readonly object _lock;

        #endregion

        #region Constructor

        public TauProxy()
        {
            _lock = new object();
        }

        #endregion

        #region ITauProxy Members

        public void Start()
        {
            lock (_lock)
            {
                if (_isRunning)
                {
                    throw new ApplicationException();
                }

                if (_isDisposed)
                {
                    throw new NotImplementedException();
                }
            }

            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
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

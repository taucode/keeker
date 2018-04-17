using Keeker.Convey.Conf;
using Keeker.Convey.Listeners;
using Keeker.Convey.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Keeker.Convey.Proxies
{
    public class Proxy : IProxy
    {
        #region Logging

        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        #endregion

        #region Fields

        private bool _isRunning;
        private bool _isDisposed;
        private readonly object _lock;
        private readonly Dictionary<string, CertificateInfo> _certificates;
        private readonly List<Listener> _listeners;

        #endregion

        #region Constructor

        public Proxy(ProxyPlainConf conf)
        {
            _lock = new object();

            _certificates = conf.Certificates
                .ToDictionary(
                    x => x.Id,
                    x => new CertificateInfo(
                        x.Domains
                            .Select(y => y.Name)
                            .ToArray(),
                        new X509Certificate2(
                            x.FilePath,
                            x.Password)));

            _listeners = conf.Listeners
                .Select(x => new Listener(
                    x,
                    x.GetUserCertificateIds()
                        .Select(y => _certificates[y])
                        .ToArray()))
                .ToList();
        }

        #endregion

        #region IProxy Members

        public void Start()
        {
            lock (_lock)
            {
                try
                {
                    if (_isRunning)
                    {
                        throw new InvalidOperationException("Proxy already running");
                    }

                    if (_isDisposed)
                    {
                        throw new ObjectDisposedException("Proxy");
                    }

                    _isRunning = true;

                    Logger.InfoFormat("Starting the proxy");

                    try
                    {
                        foreach (var listener in _listeners)
                        {
                            listener.Start();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorException("Could not start listeners", ex);

                        foreach (var listener in _listeners)
                        {
                            try
                            {
                                listener.Dispose();
                            }
                            catch
                            {
                                // dismiss
                            }
                        }

                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Logger.ErrorException("Error occured while starting proxy", ex);
                    throw;
                }
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                try
                {
                    if (!_isRunning)
                    {
                        throw new InvalidOperationException("Proxy not running");
                    }

                    if (_isDisposed)
                    {
                        throw new ObjectDisposedException("Proxy");
                    }

                    _isRunning = false;

                    Logger.Info("Stopping the proxy");

                    throw new NotImplementedException();
                }
                catch (Exception ex)
                {
                    Logger.ErrorException("Error occured while stopping proxy", ex);
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

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}

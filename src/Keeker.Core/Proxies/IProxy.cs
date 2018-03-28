using Keeker.Core.Conf;
using Keeker.Core.Events;
using Keeker.Core.Listeners;
using System;
using System.IO;

namespace Keeker.Core.Proxies
{
    public interface IProxy : IDisposable
    {
        ProxyPlainConf GetConf();

        void Start();

        void Stop();

        bool IsRunning { get; }

        IListener[] GetListeners();

        StreamWriter Log { get; set; }

        event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        event EventHandler<RelayEventArgs> RelayStarted;

        event EventHandler<RelayEventArgs> RelayDisposed;
    }
}

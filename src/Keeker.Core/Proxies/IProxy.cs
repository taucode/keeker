using Keeker.Core.Conf;
using Keeker.Core.Events;
using System;

namespace Keeker.Core.Proxies
{
    public interface IProxy
    {
        ProxyPlainConf GetConf();

        void Start();

        void Stop();

        event EventHandler<ConnectionAcceptedEventArgs> ListenerConnectionAccepted;

        event EventHandler<RelayEventArgs> ListenerRelayCreated;
    }
}

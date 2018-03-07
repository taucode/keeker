using Keeker.Core.Conf;
using Keeker.Core.EventData;
using System;

namespace Keeker.Core
{
    public interface IProxy
    {
        ProxyPlainConf GetConf();

        void Start();

        void Stop();

        //event EventHandler Started;

        //event EventHandler Stopped;

        event EventHandler<ConnectionAcceptedEventArgs> ListenerConnectionAccepted;
    }
}

﻿using Keeker.Core.Listeners;
using System;

namespace Keeker.Server
{
    public interface IHttpServer : IDisposable
    {
        IStreamListener StreamListener { get; }

        void Start();
        
        string[] Hosts { get; }

        bool IsRunning { get; }

        event EventHandler<ServerConnection> ConnectionAccepted;

        bool IsDisposed { get; }
    }
}

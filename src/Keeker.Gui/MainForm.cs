﻿using Keeker.Core;
using System;
using System.Windows.Forms;
using TauCode.Utils.Win32;

namespace Keeker.Gui
{
    public partial class MainForm : Form
    {
        //private TcpListener _listener;
        //private X509Certificate _certificate;

        private IProxy _proxy;

        public MainForm()
        {
            InitializeComponent();

            ConsoleHelper.Initialize();

            var conf = ProxyPlainConf.LoadFromAppConfig("proxy");
            _proxy = new Proxy(conf);

            _proxy.Started += proxy_Started;
            _proxy.Stopped += proxy_Stopped;
            _proxy.ConnectionAccepted += proxy_ConnectionAccepted;
        }

        private void proxy_Started(object sender, EventArgs e)
        {
            var proxy = (IProxy)sender;
            var conf = proxy.GetConf();
            Console.WriteLine($"Started at {conf.Address}:{conf.Port}");
        }

        private void proxy_Stopped(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void proxy_ConnectionAccepted(object sender, ConnectionAcceptedEventArgs e)
        {
            var client = e.TcpClient;
            Console.WriteLine($"Established connection at {client.Client.RemoteEndPoint}");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            _proxy.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.buttonStart_Click(sender, e);
        }
    }
}

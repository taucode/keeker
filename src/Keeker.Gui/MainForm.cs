using Keeker.Core;
using Keeker.Core.Conf;
using Keeker.Core.EventData;
using Keeker.Core.TheDevices;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Keeker.Gui
{
    public partial class MainForm : Form
    {
        private readonly IProxy _proxy;
        private readonly ProxyPlainConf _conf;
        private readonly Dictionary<string, Dictionary<string, TheDevice>> _theDeviceCollectionsByListenerId;
        private readonly object _lock;

        public MainForm()
        {
            InitializeComponent();

            _lock = new object();
            _theDeviceCollectionsByListenerId = new Dictionary<string, Dictionary<string, TheDevice>>();

            _conf = ProxyPlainConf.LoadFromAppConfig("proxy");
            _proxy = new Proxy(_conf);

            _proxy.ListenerConnectionAccepted += proxy_ListenerConnectionAccepted;
            _proxy.ListenerTheDeviceCreated += proxy_ListenerTheDeviceCreated;

            //_proxy.Started += proxy_Started;
            //_proxy.Stopped += proxy_Stopped;
            //_proxy.ConnectionAccepted += proxy_ConnectionAccepted;
        }

        private void proxy_ListenerTheDeviceCreated(object sender, TheDeviceEventArgs e)
        {
            this.RememberTheDevice(e.TheDevice);
        }

        private void RememberTheDevice(TheDevice theDevice)
        {
            lock (_lock)
            {
                var containsListener = _theDeviceCollectionsByListenerId.TryGetValue(theDevice.ListenerId, out var theDevices);
                if (!containsListener)
                {
                    theDevices = new Dictionary<string, TheDevice>();
                }

                if (containsListener)
                {
                    theDevices.Add(theDevice.Id, theDevice);
                }
            }
        }

        private void proxy_ListenerConnectionAccepted(object sender, ConnectionAcceptedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        //private void proxy_Started(object sender, EventArgs e)
        //{
        //    var proxy = (IProxy)sender;
        //    var conf = proxy.GetConf();
        //    Console.WriteLine($"Started at {conf.Address}:{conf.Port}");
        //}

        //private void proxy_Stopped(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void proxy_ConnectionAccepted(object sender, ConnectionAcceptedEventArgs e)
        //{
        //    var client = e.TcpClient;
        //    Console.WriteLine($"Established connection at {client.Client.RemoteEndPoint}");
        //}

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                _proxy.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            listViewListeners.Columns[1].Width = 100;
            listViewHosts.Columns[2].Width = 100;
            this.ReflectListeners();

            this.buttonStart_Click(sender, e);
        }

        private void ReflectListeners()
        {
            foreach (var listenerConf in _conf.Listeners.Values)
            {
                var item = new ListViewItem();
                listViewListeners.Items.Add(item);
                item.SubItems[0].Text = listenerConf.Id;
                item.SubItems.Add(listenerConf.EndPoint.ToString());
                item.SubItems.Add(listenerConf.IsHttps.ToString());
                item.SubItems.Add(listenerConf.Hosts.Count.ToString());
            }

            if (listViewListeners.Items.Count > 0)
            {
                listViewListeners.Items[0].Selected = true;
            }
        }

        private void listViewListeners_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = listViewListeners.GetSingleSelectedItem();

            if (item == null)
            {
                this.ClearHosts();
            }
            else
            {
                this.ReflectHosts(item.SubItems[0].Text);
            }
        }

        private void ReflectHosts(string listenerId)
        {
            var listenerConf = _conf.Listeners[listenerId];
            listViewHosts.Items.Clear();

            foreach (var hostConf in listenerConf.Hosts.Values)
            {
                var item = new ListViewItem();
                item.Text = hostConf.ExternalHostName;
                item.SubItems.Add(hostConf.DomesticHostName);
                item.SubItems.Add(hostConf.EndPoint.ToString());
                item.SubItems.Add(hostConf.CertificateId);

                listViewHosts.Items.Add(item);
            }
        }

        private void ClearHosts()
        {
            listViewHosts.Items.Clear();
        }
    }
}

using Keeker.Core;
using Keeker.Core.Conf;
using Keeker.Core.EventData;
using Keeker.Core.Relays;
using Keeker.Gui.Data;
using Keeker.Gui.Panes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Keeker.Gui
{
    public partial class MainForm : Form
    {
        private readonly IProxy _proxy;
        private readonly ProxyPlainConf _conf;
        private readonly Dictionary<string, Dictionary<string, Relay>> _relayCollectionsByListenerId;
        private readonly object _lock;
        private Pane _currentPane;

        public MainForm()
        {
            InitializeComponent();

            _lock = new object();
            _relayCollectionsByListenerId = new Dictionary<string, Dictionary<string, Relay>>();

            _conf = ProxyPlainConf.LoadFromAppConfig("proxy");
            _proxy = new Proxy(_conf);

            _proxy.ListenerConnectionAccepted += proxy_ListenerConnectionAccepted;
            _proxy.ListenerRelayCreated += proxy_ListenerRelayCreated;

            //_proxy.Started += proxy_Started;
            //_proxy.Stopped += proxy_Stopped;
            //_proxy.ConnectionAccepted += proxy_ConnectionAccepted;
        }

        private void proxy_ListenerRelayCreated(object sender, RelayEventArgs e)
        {
            this.RememberRelay(e.Relay);
        }

        private void RememberRelay(Relay relay)
        {
            lock (_lock)
            {
                var containsListener = _relayCollectionsByListenerId.TryGetValue(relay.ListenerId, out var relays);
                if (!containsListener)
                {
                    relays = new Dictionary<string, Relay>();
                }

                if (containsListener)
                {
                    relays.Add(relay.Id, relay);
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
            this.ReflectListeners();

            this.buttonStart_Click(sender, e);
        }

        private void ReflectListeners()
        {
            foreach (var listenerConf in _conf.Listeners.Values)
            {
                var node = new TreeNode(listenerConf.Id)
                {
                    ImageIndex = 0,
                    SelectedImageIndex = 0,
                    Tag = listenerConf.ToListenerConfDto(),
                };

                treeViewRelays.Nodes.Add(node);

                foreach (var hostConf in listenerConf.Hosts.Values)
                {
                    this.ReflectHost(node, hostConf);
                }

                //var item = new ListViewItem();
                //listViewListeners.Items.Add(item);
                //item.SubItems[0].Text = listenerConf.Id;
                //item.SubItems.Add(listenerConf.EndPoint.ToString());
                //item.SubItems.Add(listenerConf.IsHttps.ToString());
                //item.SubItems.Add(listenerConf.Hosts.Count.ToString());
            }

            //if (listViewListeners.Items.Count > 0)
            //{
            //    listViewListeners.Items[0].Selected = true;
            //}
        }

        private void ReflectHost(TreeNode listenerNode, HostPlainConf hostConf)
        {
            var node = new TreeNode(hostConf.ExternalHostName)
            {
                ImageIndex = 1,
                SelectedImageIndex = 1,
                Tag = hostConf.ToHostConfDto(),
            };

            listenerNode.Nodes.Add(node);
        }

        private void treeViewRelays_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = e.Node;
            if (node.Tag is ListenerConfDto)
            {
                var conf = (ListenerConfDto)node.Tag;
                this.SetPane(new JsonPropertiesPane(conf));
            }
            else if (node.Tag is HostConfDto)
            {
                var conf = (HostConfDto)node.Tag;
                this.SetPane(new JsonPropertiesPane(conf));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void SetPane(Pane pane)
        {
            pane.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            pane.Width = panelPane.Width;
            pane.Height = panelPane.Height;

            if (_currentPane != null)
            {
                panelPane.Controls.Remove(_currentPane);
                _currentPane.Dispose();
            }
            
            panelPane.Controls.Add(pane);
            //pane.Show();
            _currentPane = pane;
        }

        //private void ReflectHosts(string listenerId)
        //{
        //    var listenerConf = _conf.Listeners[listenerId];
        //    listViewHosts.Items.Clear();

        //    foreach (var hostConf in listenerConf.Hosts.Values)
        //    {
        //        var item = new ListViewItem();
        //        item.Text = hostConf.ExternalHostName;
        //        item.SubItems.Add(hostConf.DomesticHostName);
        //        item.SubItems.Add(hostConf.EndPoint.ToString());
        //        item.SubItems.Add(hostConf.CertificateId);

        //        listViewHosts.Items.Add(item);
        //    }
        //}

        //private void ClearHosts()
        //{
        //    listViewHosts.Items.Clear();
        //}
    }
}

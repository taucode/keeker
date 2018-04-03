using Keeker.Convey.Proxies;
using Keeker.Gui.Data;
using Keeker.Gui.Panes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Keeker.Gui
{
    public partial class MainForm : Form
    {
        //private readonly IProxy _proxy;
        //private readonly ProxyPlainConf _conf;
        //private readonly Dictionary<string, Dictionary<string, Relay>> _relayCollectionsByListenerId;

        private LogForm _logForm;

        private ITauProxy _proxy;
        private readonly object _lock;

        private Pane _currentPane;


        public MainForm()
        {
            InitializeComponent();

            _logForm = new LogForm();

            _lock = new object();
            //_relayCollectionsByListenerId = new Dictionary<string, Dictionary<string, Relay>>();

            //_conf = ProxyPlainConf.LoadFromAppConfig("proxy");
            //_proxy = new Proxy(_conf);

            //_proxy.ConnectionAccepted += proxy_ListenerConnectionAccepted;
            //_proxy.RelayStarted += proxy_RelayStarted;
            //_proxy.RelayDisposed += proxy_RelayDisposed;
            //_proxy.ListenerRelayCreated += proxy_ListenerRelayCreated;
        }


        //private void proxy_ListenerRelayCreated(object sender, RelayEventArgs e)
        //{
        //    this.ReflectCreatedRelay(e.Relay);
        //}

        //private void ReflectCreatedRelay(IRelay relay)
        //{
        //    lock (_lock)
        //    {
        //        this.Invoke(new Action<IRelay>(
        //            r =>
        //            {
        //                var listenerNode = this.GetListenerNode(r.ListenerId);
        //                var hostNode = this.GetHostNode(listenerNode, r.ExternalHostName);

        //                var node = new TreeNode(relay.Id)
        //                {
        //                    ImageIndex = 2,
        //                    SelectedImageIndex = 2,
        //                    Tag = relay,
        //                };

        //                hostNode.Nodes.Add(node);
        //            }),
        //            relay);

        //        //var containsListener = _relayCollectionsByListenerId.TryGetValue(relay.ListenerId, out var relays);
        //        //if (!containsListener)
        //        //{
        //        //    relays = new Dictionary<string, Relay>();
        //        //}

        //        //if (containsListener)
        //        //{
        //        //    relays.Add(relay.Id, relay);
        //        //}
        //    }
        //}

        private TreeNode GetHostNode(TreeNode listenerNode, string host)
        {
            return listenerNode.Nodes
                .Cast<TreeNode>()
                .Single(x => ((HostConfDto)x.Tag).ExternalHostName == host);
        }

        private TreeNode GetListenerNode(string listenerId)
        {
            return treeViewRelays.Nodes
                .Cast<TreeNode>()
                .Single(x => ((ListenerConfDto)x.Tag).Id == listenerId);
        }

        //private void proxy_ListenerConnectionAccepted(object sender, ConnectionAcceptedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        //private void proxy_RelayStarted(object sender, RelayEventArgs e)
        //{
        //    this.ReflectCreatedRelay(e.Relay);
        //}

        //private void proxy_RelayDisposed(object sender, RelayEventArgs e)
        //{
        //    throw new NotImplementedException();
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
            //this.ReflectListeners();

            //this.buttonStart_Click(sender, e);
        }

        private void ReflectListeners()
        {
            throw new NotImplementedException();

            //foreach (var listenerConf in _conf.Listeners.Values)
            //{
            //    var node = new TreeNode(listenerConf.Id)
            //    {
            //        ImageIndex = 0,
            //        SelectedImageIndex = 0,
            //        Tag = listenerConf.ToListenerConfDto(),
            //    };

            //    treeViewRelays.Nodes.Add(node);

            //    foreach (var hostConf in listenerConf.Hosts.Values)
            //    {
            //        this.ReflectHost(node, hostConf);
            //    }

            //    //var item = new ListViewItem();
            //    //listViewListeners.Items.Add(item);
            //    //item.SubItems[0].Text = listenerConf.Id;
            //    //item.SubItems.Add(listenerConf.EndPoint.ToString());
            //    //item.SubItems.Add(listenerConf.IsHttps.ToString());
            //    //item.SubItems.Add(listenerConf.Hosts.Count.ToString());
            //}

            //if (listViewListeners.Items.Count > 0)
            //{
            //    listViewListeners.Items[0].Selected = true;
            //}
        }

        //private void ReflectHost(TreeNode listenerNode, HostPlainConf hostConf)
        //{
        //    var node = new TreeNode(hostConf.ExternalHostName)
        //    {
        //        ImageIndex = 1,
        //        SelectedImageIndex = 1,
        //        Tag = hostConf.ToHostConfDto(),
        //    };

        //    listenerNode.Nodes.Add(node);
        //}

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
            //else if (node.Tag is IRelay)
            //{
            //    var relay = (Relay)node.Tag;
            //    this.SetPane(new RelayPane(relay));
            //}
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

        private void buttonClient_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new ClientForm();
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_proxy != null)
                {
                    throw new ApplicationException();
                }

                _proxy = new TauProxy();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                throw new NotImplementedException();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonDispose_Click(object sender, EventArgs e)
        {
            try
            {
                throw new NotImplementedException();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonLog_Click(object sender, EventArgs e)
        {
            _logForm.Show();
        }
    }
}

using Keeker.Gui.Data;
using Keeker.Gui.Panes;
using Keeker.UI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Keeker.Gui
{
    public partial class MainForm : Form
    {
        private readonly LogForm _logForm;

        //private IProxy _proxy;
        private readonly object _lock;

        private Pane _currentPane;


        public MainForm()
        {
            InitializeComponent();

            _logForm = new LogForm();

            _lock = new object();
        }

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

        private void buttonStart_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //try
            //{
            //    _proxy.Start();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            buttonCreate_Click(sender, e);
            buttonStart_Click(sender, e);
            buttonLog_Click(sender, e);
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
            throw new NotImplementedException();
            //try
            //{
            //    if (_proxy != null)
            //    {
            //        throw new ApplicationException();
            //    }

            //    var conf = (ProxySection)ConfigurationManager.GetSection("proxy");
            //    var plainConf = conf.ToPlainConf();

            //    _proxy = new Proxy(plainConf);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //try
            //{
            //    _proxy.Stop();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void buttonDispose_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //try
            //{
            //    _proxy.Dispose();
            //    _proxy = null;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void buttonLog_Click(object sender, EventArgs e)
        {
            _logForm.Show();
        }
    }
}

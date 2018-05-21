using Keeker.Core;
using Keeker.Core.Listeners;
using Keeker.Server.Impl;
using Keeker.Server.UI;
using System;
using System.Windows.Forms;

namespace Keeker.Client.Gui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonServer_Click(object sender, EventArgs e)
        {
            try
            {
                this.StartServer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StartServer()
        {
            var dlg = new EndPointDialog();
            var endPoint = dlg.ShowEndPoint("link://3333");
            if (endPoint != null)
            {
                var listener = CoreHelper.CreateListenerForEndPoint(endPoint);
                var server = new HttpServerBase(listener, new[] { "rho.me" }, Program.Instance.HandlerFactory);
                var form = new HttpServerForm(server);
                form.Show();
            }
        }

        private void buttonClient_Click(object sender, EventArgs e)
        {
            try
            {
                this.StartClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StartClient()
        {
            var dlg = new EndPointDialog();
            var endPoint = dlg.ShowEndPoint("link://3333");
            if (endPoint != null)
            {
                var stream = CoreHelper.CreateStreamFromEndPoint(endPoint);
                var client = new HttpClient(stream);
                var form = new ClientForm(client);
                form.Show();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                var streamListener = new LinkStreamListener(3333);
                var server = new HttpServerBase(streamListener, new[] { "rho.me", }, Program.Instance.HandlerFactory);
                var serverForm = new HttpServerForm(server);
                serverForm.Show();
                serverForm.ClickStartButton();

                var stream = CoreHelper.CreateStreamFromEndPoint("link://3333");
                var client = new HttpClient(stream);
                var clientForm = new ClientForm(client);
                clientForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

using Keeker.Core;
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
                var server = new HttpServerBase(listener, new[] { "rho.me" });
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
    }
}

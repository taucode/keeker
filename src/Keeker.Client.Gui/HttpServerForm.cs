using Keeker.Server;
using System;
using System.Windows.Forms;

namespace Keeker.Client.Gui
{
    public partial class HttpServerForm : Form
    {
        private readonly IHttpServer _server;

        public HttpServerForm()
        {
            InitializeComponent();
        }

        public HttpServerForm(IHttpServer server)
            : this()
        {
            _server = server;
            _server.ConnectionAccepted += server_ConnectionAccepted;
        }

        private void server_ConnectionAccepted(object sender, Connection connection)
        {
            this.Invoke(new Action(() => this.ReflectConnection(connection)));
        }

        private void ReflectConnection(Connection connection)
        {
            var item = new ListViewItem(connection.Id);
            item.SubItems.Add("con prop");
            listViewConnections.Items.Add(item);
        }

        private void HttpServerForm_Load(object sender, System.EventArgs e)
        {
            textBoxListening.Text = _server.ListenedAddress;
            textBoxHosts.Text = string.Join(", ", _server.Hosts);
        }

        private void buttonStart_Click(object sender, System.EventArgs e)
        {
            try
            {
                _server.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

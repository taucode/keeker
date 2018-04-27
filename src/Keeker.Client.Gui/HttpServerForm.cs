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

using System;
using System.Windows.Forms;

namespace Keeker.Server.UI
{
    public partial class HttpServerForm : Form
    {
        #region Fields


        private readonly IHttpServer _server;

        #endregion

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

        private void DoInvoke(Action action)
        {
            this.Invoke(action);
        }

        private void server_ConnectionAccepted(object sender, ServerConnection connection)
        {
            this.DoInvoke(() => this.ReflectConnection(connection));
        }

        private void ReflectConnection(ServerConnection connection)
        {
            var item = new ListViewItem(connection.Id);
            listViewConnections.Items.Add(item);
        }

        private void HttpServerForm_Load(object sender, System.EventArgs e)
        {
            textBoxEndPoint.Text = _server.StreamListener.LocalEndpointName;
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

        private void buttonDispose_Click(object sender, System.EventArgs e)
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
    }
}

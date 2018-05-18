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
        }
    }
}

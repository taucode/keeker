using Keeker.Server.Conf;
using Keeker.UI;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Keeker.Server.Gui
{
    public partial class MainForm : Form
    {
        private readonly LogForm _logForm;
        private IHttpServer _server;

        public MainForm()
        {
            InitializeComponent();

            _logForm = new LogForm();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_server != null)
                {
                    throw new ApplicationException();
                }

                var section = (ServerSection)ConfigurationManager.GetSection("server");
                var plainConf = section.ToPlainConf();

                var factory = this.CreateHandlerFactory();
                _server = new HttpServer(plainConf.EndPoint, factory);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private IHandlerFactory CreateHandlerFactory()
        {
            var locatoin = Assembly.GetEntryAssembly().Location;
            var path = Path.GetDirectoryName(locatoin);
            var homeDirectory = Path.Combine(path, "SampleSite");

            var staticContentResolver = new StaticContentResolver(homeDirectory);
            var factory = new HandlerFactoryBase(staticContentResolver);
            return factory;
        }

        private void buttonStart_Click(object sender, EventArgs e)
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

        private void buttonDispose_Click(object sender, EventArgs e)
        {
            try
            {
                _server.Dispose();
                _server = null;
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            buttonCreate_Click(sender, e);
            buttonStart_Click(sender, e);

            new Task(() =>
            {
                Thread.Sleep(200);
                var client = new WebClient();
                var wat = client.DownloadString("http://localhost:1488/index.html");
            }).Start();
        }
    }
}

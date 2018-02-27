using Keeker.Core;
using System;
using System.Windows.Forms;
using TauCode.Utils.Win32;

namespace Keeker.Gui
{
    public partial class MainForm : Form
    {
        //private TcpListener _listener;
        //private X509Certificate _certificate;

        private IProxy _proxy;

        public MainForm()
        {
            InitializeComponent();

            ConsoleHelper.Initialize();

            var conf = ProxyPlainConf.LoadFromAppConfig("proxy");
            _proxy = new Proxy(conf);

            _proxy.Started += proxy_Started;
            _proxy.Stopped += proxy_Stopped;
            _proxy.ConnectionAccepted += proxy_ConnectionAccepted;
        }

        private void proxy_Started(object sender, EventArgs e)
        {
            var proxy = (IProxy)sender;
            var conf = proxy.GetConf();
            Console.WriteLine($"Started at {conf.Address}:{conf.Port}");
        }

        private void proxy_Stopped(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void proxy_ConnectionAccepted(object sender, ConnectionAcceptedEventArgs e)
        {
            var client = e.TcpClient;
            Console.WriteLine($"Established connection at {client.Client.RemoteEndPoint}");
        }


        //private void AcceptConnection(TcpClient client)
        //{
        //    Console.WriteLine("Accepted connection");

        //    var clientStream = new SslStream(client.GetStream(), false);
        //    clientStream.AuthenticateAsServer(_certificate, false, SslProtocols.Tls12, false);
        //    var realStream = new TcpClient("127.0.0.1", 53808).GetStream();

        //    new Task(() => Redirect(clientStream, realStream)).Start();
        //    new Task(() => Redirect(realStream, clientStream)).Start();
        //}

        //private void Redirect(Stream from, Stream to)
        //{
        //    var buffer = new byte[65536];

        //    while (true)
        //    {
        //        try
        //        {
        //            var bytesCount = from.Read(buffer, 0, buffer.Length);
        //            to.Write(buffer, 0, bytesCount);
        //        }
        //        catch
        //        {
        //            try
        //            {
        //                from.Close();
        //            }
        //            catch
        //            {
        //                // don't care; dismiss
        //            }

        //            try
        //            {
        //                to.Close();
        //            }
        //            catch
        //            {
        //                // don't care; dismiss
        //            }

        //            // don't care, just exit.
        //            break;
        //        }
        //    }
        //}

        private void buttonStart_Click(object sender, EventArgs e)
        {
            _proxy.Start();

            //_listener.Start();
            //Console.WriteLine("Listening...");

            //new Task(() =>
            //{
            //    while (true)
            //    {
            //        try
            //        {
            //            var client = _listener.AcceptTcpClient();
            //            new Task(() => AcceptConnection(client)).Start();
            //        }
            //        catch
            //        {
            //            break;
            //        }
            //    }
            //}).Start();

            //new Task(() =>
            //{
            //    Console.WriteLine("Press enter to exit");
            //    Console.ReadLine();
            //    _listener.Stop();
            //    this.Invoke(new Action(() => this.Close()));
            //}).Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.buttonStart_Click(sender, e);
        }
    }
}

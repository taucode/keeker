using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using TauCode.Utils.Win32;

namespace Keeker.Gui
{
    public partial class MainForm : Form
    {
        private TcpListener _listener;
        private X509Certificate _certificate;

        public MainForm()
        {
            InitializeComponent();

            ConsoleHelper.Initialize();

            _listener = new TcpListener(IPAddress.Any, 443);
            _certificate = new X509Certificate(@"C:\temp\rho.ssl\certificate_combined.pfx", "doresaq1488");
        }
        
        private void AcceptConnection(TcpClient client)
        {
            Console.WriteLine("Accepted connection");

            var clientStream = new SslStream(client.GetStream(), false);
            clientStream.AuthenticateAsServer(_certificate, false, SslProtocols.Tls12, false);
            var realStream = new TcpClient("127.0.0.1", 53808).GetStream();

            new Task(() => Redirect(clientStream, realStream)).Start();
            new Task(() => Redirect(realStream, clientStream)).Start();
        }

        private void Redirect(Stream from, Stream to)
        {
            var buffer = new byte[65536];

            while (true)
            {
                try
                {
                    var bytesCount = from.Read(buffer, 0, buffer.Length);
                    to.Write(buffer, 0, bytesCount);
                }
                catch
                {
                    try
                    {
                        from.Close();
                    }
                    catch
                    {
                        // don't care; dismiss
                    }

                    try
                    {
                        to.Close();
                    }
                    catch
                    {
                        // don't care; dismiss
                    }

                    // don't care, just exit.
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _listener.Start();
            Console.WriteLine("Listening...");

            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        var client = _listener.AcceptTcpClient();
                        new Task(() => AcceptConnection(client)).Start();
                    }
                    catch
                    {
                        break;
                    }
                }
            }).Start();

            new Task(() =>
            {
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
                _listener.Stop();
                this.Invoke(new Action(() => this.Close()));
            }).Start();
        }
    }
}

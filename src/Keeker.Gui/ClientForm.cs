using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Keeker.Gui
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        private void buttonSample1_Click(object sender, EventArgs e)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    var txt = client.DownloadString("https://rho.me/");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSample2_Click(object sender, EventArgs e)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            
            socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 443));

            using (socket)
            using (var stream = new NetworkStream(socket, true))
            using (var sslStream = new SslStream(stream, false))
            {
                sslStream.AuthenticateAsClient("rho.me");

                var req = 
                    "GET / HTTP/1.1\r\n" + 
                    "Host: rho.me\r\n" +
                    "\r\n";
                var reqBytes = Encoding.ASCII.GetBytes(req);

                sslStream.Write(reqBytes);

                var buf = new byte[65536];
                var total = new List<byte>();

                var bytesRead = sslStream.Read(buf, 0, buf.Length);
                total.AddRange(buf.Take(bytesRead));

                bytesRead = sslStream.Read(buf, 0, buf.Length);
                total.AddRange(buf.Take(bytesRead));

                var response = Encoding.UTF8.GetString(total.ToArray(), 0, total.Count);
                textBoxResponse.Text = response;
            }
        }
    }
}

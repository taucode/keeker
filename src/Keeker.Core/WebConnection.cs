using System.IO;
using System.Threading.Tasks;

namespace Keeker.Core
{
    public class WebConnection
    {
        private readonly Stream _clientStream;
        private readonly Stream _serverStream;

        private readonly WebRequestRelay _relay;

        public WebConnection(Stream clientStream, Stream serverStream)
        {
            _clientStream = clientStream;
            _serverStream = serverStream;
            _relay = new WebRequestRelay(new KeekStream(_clientStream), serverStream);
        }

        public void Start()
        {
            _relay.Start();
            new Task(() => this.Redirect(_serverStream, _clientStream)).Start();
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
    }
}

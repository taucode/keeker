using System.IO;
using System.Threading.Tasks;

namespace Keeker.Core
{
    public class WebConnection
    {
        private readonly Stream _clientStream;
        private readonly Stream _serverStream;

        public WebConnection(Stream clientStream, Stream serverStream)
        {
            _clientStream = clientStream;
            _serverStream = serverStream;
        }

        public void Start()
        {
            new Task(() => Redirect(_clientStream, _serverStream)).Start();
            new Task(() => Redirect(_serverStream, _clientStream)).Start();
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

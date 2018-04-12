using Keeker.Core;
using System.Net;

namespace Keeker.Server.Conf
{
    public class ServerPlainConf
    {
        public ServerPlainConf(IPEndPoint endPoint)
        {
            this.EndPoint = endPoint;
        }

        public IPEndPoint EndPoint { get; }
    }

    public static class ServerPlainConfExtensions
    {
        public static ServerPlainConf ToPlainConf(this ServerSection section)
        {
            return new ServerPlainConf(section.EndPoint.ToIPEndPoint());
        }
    }
}

using Keeker.Core.Conf;
using System.IO;

namespace Keeker.Core.Relays
{
    public class SecureRelay : RelayBase
    {
        public SecureRelay(Stream clientStream, RelayPlainConf conf)
            : base(clientStream, conf)
        {
        }
    }
}
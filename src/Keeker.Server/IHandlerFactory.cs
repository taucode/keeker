using Keeker.Core.Data;
using System.IO;
using System.Threading;

namespace Keeker.Server
{
    public interface IHandlerFactory
    {
        IHandler CreateHandler(string connectionId, HttpRequestMetadata requestMetadata, Stream stream, ManualResetEvent stopSignal);
    }
}

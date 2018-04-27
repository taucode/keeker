using System.IO;

namespace Keeker.Core.Streams
{
    public interface IByteSender
    {
        void Send(byte[] buffer, int offset, int count);

        Stream TargetStream { get; }
    }
}

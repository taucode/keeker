using System.IO;

namespace Keeker.Core.ByteSenders
{
    internal interface IByteSender
    {
        void Send(byte[] buffer, int offset, int count);

        Stream TargetStream { get; }
    }
}

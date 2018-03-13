using Keeker.Core.TheDevices;
using System;

namespace Keeker.Core.EventData
{
    public class TheDeviceEventArgs : EventArgs
    {
        public TheDeviceEventArgs(TheDevice theDevice)
        {
            this.TheDevice = theDevice;
        }

        public TheDevice TheDevice { get; }
    }
}

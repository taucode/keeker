using System.Collections.Generic;

namespace Keeker.Gui.Data
{
    public class ListenerConfDto
    {
        public string Id { get; set; }
        public string EndPoint { get; set; }
        public bool IsHttps { get; set; }
        public Dictionary<string, HostConfDto> Hosts { get; set; }
    }
}

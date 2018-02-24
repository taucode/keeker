using System.Collections.Generic;
using System.Net;

namespace Keeker.Core
{
    public class PlainKeekerSection
    {
        public IPAddress IpAddress { get; set; }
        public int Port { get; set; }

        public class HostEntry
        {
            public string Name { get; set; }
            public TargetEntry[] Targets { get; set; }
            public Certificate Certificate { get; set; }
        }

        public class TargetEntry
        {
            public string Name { get; set; }
            public bool IsActive { get; set; }
            public string Host { get; set; }
            public IPAddress IpAddress { get; set; }
            public int Port { get; set; }
        }

        public class Certificate
        {
            public string FilePath { get; set; }
            public string Password { get; set; }
        }

        public Dictionary<string, HostEntry> Hosts { get; set; }
    }
}

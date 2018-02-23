using System.Collections.Generic;

namespace Keeker.Core
{
    public class PlainConfiguration
    {
        public class Host
        {
            public string Name { get; }
            public Target[] Targets { get; }
            public Cert Cert { get; }
        }

        public class Target
        {
            public string Name { get; set; }
            public bool IsActive { get; }
            public string Host { get; }
            public string IpAddress { get; }
            public int Port { get; }
        }

        public class Cert
        {
            public string FilePath { get; }
            public string Password { get; }
        }

        public Dictionary<string, Host> Hosts { get; }
    }
}

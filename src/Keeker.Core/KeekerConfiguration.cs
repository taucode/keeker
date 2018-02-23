using System.Configuration;

namespace Keeker.Core
{
    public class KeekerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("hosts", IsDefaultCollection = true)]
        public HostElementCollection Hosts
        {
            get => this["hosts"] as HostElementCollection;
            set => this["hosts"] = value;
        }
    }

    [ConfigurationCollection(typeof(HostElement))]
    public class HostElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HostElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HostElement)element).Name;
        }
    }

    public class HostElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }

        [ConfigurationProperty("targets", IsDefaultCollection = true)]
        public TargetElementCollection Targets
        {
            get => this["targets"] as TargetElementCollection;
            set => this["targets"] = value;
        }

        [ConfigurationProperty("cert", IsRequired = true)]
        public CertElement Cert
        {
            get => (CertElement)this["cert"];
            set => this["name"] = value;
        }
    }

    [ConfigurationCollection(typeof(TargetElement))]
    public class TargetElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TargetElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TargetElement)element).Name;
        }
    }

    public class TargetElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }

        [ConfigurationProperty("isActive", DefaultValue = "false", IsRequired = true)]
        public bool IsActive
        {
            get => (bool)this["isActive"];
            set => this["isActive"] = value;
        }

        [ConfigurationProperty("host", DefaultValue = "", IsRequired = true)]
        public string Host
        {
            get => (string)this["host"];
            set => this["host"] = value;
        }

        [ConfigurationProperty("ipAddress", DefaultValue = "", IsRequired = true)]
        public string IpAddress
        {
            get => (string)this["ipAddress"];
            set => this["ipAddress"] = value;
        }

        [ConfigurationProperty("port", DefaultValue = "", IsRequired = true)]
        public int Port
        {
            get => (int)this["port"];
            set => this["port"] = value;
        }
    }

    public class CertElement : ConfigurationElement
    {
        [ConfigurationProperty("filePath", DefaultValue = "", IsRequired = true)]
        public string FilePath
        {
            get => (string)this["filePath"];
            set => this["filePath"] = value;
        }

        [ConfigurationProperty("password", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Password
        {
            get => (string)this["password"];
            set => this["password"] = value;
        }
    }
}

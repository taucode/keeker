using System.Configuration;

namespace Keeker.Core.Conf
{
    public class ProxySection : ConfigurationSection
    {
        [ConfigurationProperty("listeners", IsDefaultCollection = true)]
        public ListenerElementCollection Listeners
        {
            get => this["listeners"] as ListenerElementCollection;
            set => this["listeners"] = value;
        }
    }

    [ConfigurationCollection(typeof(ListenerElement))]
    public class ListenerElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ListenerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ListenerElement)element).Id;
        }
    }

    public class ListenerElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public string Id
        {
            get => (string)this["id"];
            set => this["id"] = value;
        }

        [ConfigurationProperty("address", IsRequired = true)]
        public string Address
        {
            get => (string)this["address"];
            set => this["address"] = value;
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get => (int)this["port"];
            set => this["port"] = value;
        }

        [ConfigurationProperty("isHttps", IsRequired = true)]
        public bool IsHttps
        {
            get => (bool)this["isHttps"];
            set => this["isHttps"] = value;
        }

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
            return ((HostElement)element).ExternalHostName;
        }
    }

    public class HostElement : ConfigurationElement
    {
        [ConfigurationProperty("externalHostName", IsKey = true, IsRequired = true)]
        public string ExternalHostName
        {
            get => (string)this["externalHostName"];
            set => this["externalHostName"] = value;
        }

        [ConfigurationProperty("httpRedirect", IsRequired = false, DefaultValue = null)]
        public HttpRedirectElement HttpRedirect
        {
            get => (HttpRedirectElement)this["httpRedirect"];
            set => this["httpRedirect"] = value;
        }

        [ConfigurationProperty("relay", IsRequired = false, DefaultValue = null)]
        public RelayElement Relay
        {
            get => (RelayElement)this["relay"];
            set => this["relay"] = value;
        }

        [ConfigurationProperty("certificate", IsRequired = false, DefaultValue = null)]
        public CertificateElement Certificate
        {
            get => (CertificateElement)this["certificate"];
            set => this["certificate"] = value;
        }
    }

    public class HttpRedirectElement : ConfigurationElement
    {
        [ConfigurationProperty("toHostName", IsRequired = true)]
        public string ToHostName
        {
            get => (string)this["toHostName"];
            set => this["toHostName"] = value;
        }
    }

    public class RelayElement : ConfigurationElement
    {
        [ConfigurationProperty("domesticHostName", IsRequired = true)]
        public string DomesticHostName
        {
            get => (string)this["domesticHostName"];
            set => this["domesticHostName"] = value;
        }

        [ConfigurationProperty("address", IsRequired = true)]
        public string Address
        {
            get => (string)this["address"];
            set => this["address"] = value;
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get => (int)this["port"];
            set => this["port"] = value;
        }
    }

    public class CertificateElement : ConfigurationElement
    {
        [ConfigurationProperty("filePath", IsRequired = true)]
        public string FilePath
        {
            get => (string)this["filePath"];
            set => this["filePath"] = value;
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get => (string)this["password"];
            set => this["password"] = value;
        }
    }
}

using System.Configuration;

namespace Keeker.Core.Conf
{
    public class ProxySection : ConfigurationSection
    {
        [ConfigurationProperty("certificates")]
        public CertificateElementCollection Certificates
        {
            get => this["certificates"] as CertificateElementCollection;
            set => this["certificates"] = value;
        }

        [ConfigurationProperty("listeners")]
        public ListenerElementCollection Listeners
        {
            get => this["listeners"] as ListenerElementCollection;
            set => this["listeners"] = value;
        }
    }

    [ConfigurationCollection(typeof(CertificateElement))]
    public class CertificateElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CertificateElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CertificateElement)element).Id;
        }
    }

    public class CertificateElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public string Id
        {
            get => (string)this["id"];
            set => this["id"] = value;
        }

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

        [ConfigurationProperty("domains")]
        public DomainElementCollection Domains
        {
            get => this["domains"] as DomainElementCollection;
            set => this["domains"] = value;
        }
    }

    [ConfigurationCollection(typeof(DomainElement))]
    public class DomainElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DomainElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DomainElement)element).Name;
        }
    }

    public class DomainElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get => (string)this["name"];
            set => this["name"] = value;
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

        [ConfigurationProperty("endPoint", IsRequired = true)]
        public string EndPoint
        {
            get => (string)this["endPoint"];
            set => this["endPoint"] = value;
        }

        [ConfigurationProperty("isHttps", IsRequired = true)]
        public bool IsHttps
        {
            get => (bool)this["isHttps"];
            set => this["isHttps"] = value;
        }

        [ConfigurationProperty("hosts")]
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

        [ConfigurationProperty("domesticHostName", IsRequired = true)]
        public string DomesticHostName
        {
            get => (string)this["domesticHostName"];
            set => this["domesticHostName"] = value;
        }

        [ConfigurationProperty("endPoint", IsRequired = true)]
        public string EndPoint
        {
            get => (string)this["endPoint"];
            set => this["endPoint"] = value;
        }

        [ConfigurationProperty("certificateId")]
        public string CertificateId
        {
            get => (string)this["certificateId"];
            set => this["certificateId"] = value;
        }
    }
}

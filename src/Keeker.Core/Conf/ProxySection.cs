using System.Configuration;

namespace Keeker.Core.Conf
{
    public class ProxySection : ConfigurationSection
    {
        [ConfigurationProperty("certificates", IsDefaultCollection = true)]
        public CertificateElementCollection Certificates
        {
            get => this["certificates"] as CertificateElementCollection;
            set => this["certificates"] = value;
        }

        [ConfigurationProperty("listeners", IsDefaultCollection = true)]
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

        [ConfigurationProperty("relays", IsDefaultCollection = true)]
        public RelayElementCollection Relays
        {
            get => this["relays"] as RelayElementCollection;
            set => this["relays"] = value;
        }
    }

    [ConfigurationCollection(typeof(RelayElement))]
    public class RelayElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RelayElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RelayElement)element).ExternalHostName;
        }
    }

    public class RelayElement : ConfigurationElement
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

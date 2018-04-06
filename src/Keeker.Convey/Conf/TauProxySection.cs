using System.Configuration;

namespace Keeker.Convey.Conf
{
    public class TauProxySection : ConfigurationSection
    {
        [ConfigurationProperty("certificates")]
        public TauCertificateElementCollection Certificates
        {
            get => this["certificates"] as TauCertificateElementCollection;
            set => this["certificates"] = value;
        }

        [ConfigurationProperty("listeners")]
        public TauListenerElementCollection Listeners
        {
            get => this["listeners"] as TauListenerElementCollection;
            set => this["listeners"] = value;
        }
    }

    [ConfigurationCollection(typeof(TauCertificateElement))]
    public class TauCertificateElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TauCertificateElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TauCertificateElement)element).Id;
        }
    }

    public class TauCertificateElement : ConfigurationElement
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
        public TauDomainElementCollection Domains
        {
            get => this["domains"] as TauDomainElementCollection;
            set => this["domains"] = value;
        }
    }

    [ConfigurationCollection(typeof(TauDomainElement))]
    public class TauDomainElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TauDomainElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TauDomainElement)element).Name;
        }
    }

    public class TauDomainElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }
    }

    [ConfigurationCollection(typeof(TauListenerElement))]
    public class TauListenerElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TauListenerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TauListenerElement)element).Id;
        }
    }

    public class TauListenerElement : ConfigurationElement
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
        public TauHostElementCollection Hosts
        {
            get => this["hosts"] as TauHostElementCollection;
            set => this["hosts"] = value;
        }
    }

    [ConfigurationCollection(typeof(TauHostElement))]
    public class TauHostElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TauHostElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TauHostElement)element).ExternalHostName;
        }
    }

    public class TauHostElement : ConfigurationElement
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

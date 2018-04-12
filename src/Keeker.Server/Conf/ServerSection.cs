using System.Configuration;

namespace Keeker.Server.Conf
{
    public class ServerSection : ConfigurationSection
    {
        [ConfigurationProperty("endPoint", IsRequired = true)]
        public string EndPoint
        {
            get => (string)this["endPoint"];
            set => this["endPoint"] = value;
        }
    }
}

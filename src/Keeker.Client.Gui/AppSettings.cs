using System.Collections.Generic;

namespace Keeker.Client.Gui
{
    public class AppSettings
    {
        public string Method { get; set; }

        public string Uri { get; set; }

        public List<string> RecentUris { get; set; }

        public List<HttpHeaderDto> Headers { get; set; }

        public class HttpHeaderDto
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}

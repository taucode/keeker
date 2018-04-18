using System.Collections.Generic;

namespace Keeker.Client.Gui
{
    public class AppSettings
    {
        public string LastMethod { get; set; }

        public string LastUri { get; set; }

        public List<string> RecentUris { get; set; }

        public List<HttpHeaderDto> LastHeaders { get; set; }

        public class HttpHeaderDto
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}

using Keeker.Core;
using System.IO;

namespace Keeker.Server
{
    public class StaticContentResolver : IStaticContentResolver
    {
        private readonly string _homeDirectory;

        public StaticContentResolver(string homeDirectory)
        {
            _homeDirectory = homeDirectory;
        }

        public StaticContentInfo Resolve(string uri)
        {
            if (uri.StartsWith("/"))
            {
                var tail = uri.Substring(1);
                var filePath = Path.Combine(_homeDirectory, tail);
                var ext = Path.GetExtension(filePath);

                if (File.Exists(filePath))
                {
                    var contentType = this.DetectContentType(ext);
                    if (contentType != null)
                    {
                        return new StaticContentInfo
                        {
                            FilePath = filePath,
                            ContentType = contentType,
                        };
                    }
                }
            }

            return null;
        }

        private string DetectContentType(string ext)
        {
            ext = ext.ToLower();

            switch (ext)
            {
                case ".html":
                    return CoreHelper.ContentTypes.TextHtml;

                default:
                    return null;
            }
        }
    }
}

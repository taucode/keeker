using Keeker.Core.Data;

namespace Keeker.Client.Gui
{
    public static class Helper
    {
        public static AppSettings.HttpHeaderDto HttpHeaderToDto(this HttpHeader header)
        {
            var dto = new AppSettings.HttpHeaderDto
            {
                Name = header.Name,
                Value = header.Value,
            };

            return dto;
        }
    }
}
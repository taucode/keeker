using Keeker.Core.Data;
using System;
using System.Windows.Forms;

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

        public static void DoLater(Action action, int timeout)
        {
            var timer = new Timer();
            timer.Interval = timeout;
            timer.Tick += (sender, e) =>
            {
                timer.Dispose();
                action();
            };
            timer.Start();
        }
    }
}
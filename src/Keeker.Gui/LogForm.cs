using Keeker.Gui.Controls;
using Keeker.Gui.Logging;
using Serilog;
using System;
using System.Windows.Forms;

namespace Keeker.Gui
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();

            var textBoxWriter = new TextBoxWriter(textBoxLog);

            var log = new LoggerConfiguration()
                .WriteTo.TextWriter(textBoxWriter, outputTemplate: "{Timestamp:HH:mm} [{Level}] ({Name:l}) {Message}{NewLine}{Exception}")
                .CreateLogger();
            Log.Logger = log;
        }

        private void buttonLogInfo_Click(object sender, System.EventArgs e)
        {
            try
            {
                LogProvider.GetCurrentClassLogger().InfoFormat("{@Wat}", new { Id = 1, Name = "Olya" });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

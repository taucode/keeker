using Serilog;
using System.Windows.Forms;

namespace Keeker.UI
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

        private void buttonClear_Click(object sender, System.EventArgs e)
        {
            textBoxLog.Text = string.Empty;
        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public void StopLogging()
        {
            Log.Logger = null;
        }
    }
}

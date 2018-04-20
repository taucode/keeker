using System.Windows.Forms;

namespace Keeker.UI
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();

            this.TextWriter = new TextBoxWriter(textBoxLog);
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

        public TextBoxWriter TextWriter { get; }
    }
}

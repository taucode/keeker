using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Keeker.Client.Gui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.InitMethods();
        }

        private void InitMethods()
        {
            var methods = new []
            {
                "GET",
                "POST",
                "PUT",
                "PATCH",
                "DELETE",
            };

            comboBoxMethod.DataSource = methods;
            comboBoxMethod.SelectedIndex = 0;
        }

        private void comboBoxUri_TextChanged(object sender, EventArgs e)
        {
            Trace.TraceInformation(comboBoxUri.Text);
        }
    }
}

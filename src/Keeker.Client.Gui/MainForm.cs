using System;
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
            textBoxSample.Text = @"00 ff de e1 00 00 00 00 ff de e1 00 00 00 00 00  AAAiAAAiAAAiAAAi
00 ff de e1 00 00 00 00 ff de e1 00 00 00 00 00  AAAiAAAiAAAiAAAi
ee f7 63                                         x9x
";

            var bytes = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                bytes[i] = (byte)i;
            }

            // АБВ

            binaryView1.Bytes = bytes;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var s = ((char)1).ToString();
            var n = 33;
        }
    }
}

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
            textBoxSample.Text = @"
00 ff de e1 00 00 00 00 ff de e1 00 00 00 00 00  AAAiAAAiAAAiAAAi
00 ff de e1 00 00 00 00 ff de e1 00 00 00 00 00  ................
";

            binaryView1.Bytes = new byte[]
            {
                0x00, 0xff, 0xde, 0xe1, 0x00, 0x00, 0x00, 0x00, 0xff, 0xde, 0xe1, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0xff, 0xde, 0xe1, 0x00, 0x00, 0x00, 0x00, 0xff, 0xde, 0xe1, 0x00, 0x00, 0x00, 0x00, 0x00,
            };
        }
    }
}

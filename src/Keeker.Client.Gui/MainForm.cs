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

            binaryView1.Bytes = new byte[]
            {
                0x00, 0xff, 0xde, 0xe1, 0x00, 0x00, 0x00, 0x00, 0xff, 0xde, 0xe1, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0xff, 0xde, 0xe1, 0x00, 0x00, 0x00, 0x00, 0xff, 0xde, 0xe1, 0x00, 0x00, 0x00, 0x00, 0x00,
                0xee, 0xf7, 0x63,
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            binaryView1.Wat = textBox1.Text;
        }
    }
}

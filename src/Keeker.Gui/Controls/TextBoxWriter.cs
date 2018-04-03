using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Keeker.Gui.Controls
{
    public class TextBoxWriter : TextWriter
    {
        private readonly TextBox _textBox;

        public TextBoxWriter(TextBox textBox)
        {
            _textBox = textBox;
        }

        public override void Write(char value)
        {
            var sb = new StringBuilder(_textBox.Text);
            sb.Append(value);
            _textBox.Text = sb.ToString();
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}

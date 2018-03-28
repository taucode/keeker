using System;
using System.IO;
using System.Windows.Forms;

namespace Keeker.Gui.Controls
{
    public class TextBoxStreamWriter : StreamWriter
    {
        private readonly TextBox _textBox;

        public TextBoxStreamWriter(TextBox textBox, Stream stream)
            : base(stream)
        {
            _textBox = textBox;
        }

        public override void Write(char value)
        {
            throw new NotImplementedException();
        }
    }
}

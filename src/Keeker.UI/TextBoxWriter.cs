using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Keeker.UI
{
    public class TextBoxWriter : TextWriter
    {
        private readonly TextBox _textBox;
        private readonly Action<string> _setTextAction;

        public TextBoxWriter(TextBox textBox)
        {
            _textBox = textBox;
            _setTextAction = this.SetText;
        }

        public override void Write(string value)
        {
            var sb = new StringBuilder(_textBox.Text);
            sb.Append(value);
            var text = sb.ToString();

            if (_textBox.InvokeRequired)
            {
                _textBox.Invoke(_setTextAction, text);
            }
            else
            {
                _setTextAction(text);
            }
            
        }

        public override void Write(char value)
        {
            var sb = new StringBuilder(_textBox.Text);
            sb.Append(value);
            var text = sb.ToString();

            if (_textBox.InvokeRequired)
            {
                _textBox.Invoke(_setTextAction, text);
            }
            else
            {
                _setTextAction(text);
            }
        }

        private void SetText(string text)
        {
            _textBox.Text = text;
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}

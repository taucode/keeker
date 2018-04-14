using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Keeker.Client.Gui
{
    public partial class BinaryView : CustomScrollableControl
    {
        private const string DEFAULT_MONOSPACE_FONT_FAMILY_NAME = "Consolas";
        private const string RESERVE_MONOSPACE_FONT_FAMILY_NAME = "Courier New";

        private const int BYTES_PER_LINE = 16;

        private const int LEFT_MARGIN = 4;

        private byte[] _bytes;

        private Font _font;

        private int _fontWidth;
        private int _fontHeight;


        public BinaryView()
        {
            InitializeComponent();

            var font = this.CreateStandardMonospaceFont();
            this.SetFont(font);

            _bytes = new byte[0];
        }

        [Browsable(false)]
        public byte[] Bytes
        {
            get => _bytes;
            set
            {
                _bytes = value ?? throw new ArgumentNullException(nameof(value)); 
                this.Invalidate();
            }
        }

        public override Font Font
        {
            get => _font;
            set => this.SetFont(value);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            var lineCount = this.GetLineCount();

            for (int i = 0; i < lineCount; i++)
            {
                this.DrawLine(e.Graphics, i);
            }

            base.OnPaint(e);
        }

        private void DrawLine(Graphics g, int lineIndex)
        {
            var y = _fontHeight * lineIndex;
            var x = 0;

            x += LEFT_MARGIN;

            g.DrawString();
            
        }

        private int GetLineCount()
        {
            var count = _bytes.Length / BYTES_PER_LINE;
            if (_bytes.Length % BYTES_PER_LINE > 0)
            {
                count++;
            }

            return count;
        }

        private void SetFont(Font font)
        {
            if (font == null)
            {
                throw new ArgumentNullException(nameof(font));
            }

            if (font.IsMonospace())
            {
                font = (Font)font.Clone();
            }
            else
            {
                font = this.CreateStandardMonospaceFont();
            }

            _font?.Dispose();
            _font = font;

            _fontHeight = _font.Height;
            _fontWidth = (int)_font.GetCharSize('M').Width;
        }

        private Font CreateStandardMonospaceFont()
        {
            var font = new Font(DEFAULT_MONOSPACE_FONT_FAMILY_NAME, 9.75f);
            if (!font.IsMonospace())
            {
                font = new Font(RESERVE_MONOSPACE_FONT_FAMILY_NAME, 9.75f);
            }

            if (!font.IsMonospace())
            {
                throw new ApplicationException(); // todo2[ak]
            }

            return font;
        }
    }
}

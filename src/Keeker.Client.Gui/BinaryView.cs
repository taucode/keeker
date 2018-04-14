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

        private readonly Brush _offsetBrush;


        public BinaryView()
        {
            InitializeComponent();

            var font = this.CreateStandardMonospaceFont();
            this.SetFont(font);

            _bytes = new byte[0];
            _offsetBrush = new SolidBrush(Color.Black);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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

        private string _wat;
        public string Wat
        {
            get { return _wat; }
            set
            {
                _wat = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            var s = _wat ?? "";

            TextRenderer.DrawText(e.Graphics, s, _font, new Point(0, 0), Color.Black);

            var penWidth = new Pen(Color.Green);
            var penMeasure = new Pen(Color.Red);

            var width = _fontWidth * s.Length;
            var measure = TextRenderer.MeasureText(e.Graphics, s, _font).Width;
            

            e.Graphics.DrawLine(penWidth, 0, 20, width, 20);
            e.Graphics.DrawLine(penMeasure, 0, 25, measure, 25);

            //var lineCount = this.GetLineCount();

            //for (int i = 0; i < lineCount; i++)
            //{
            //    this.DrawLine(e.Graphics, i);
            //}

            //base.OnPaint(e);
        }

        private void DrawLine(Graphics g, int lineIndex)
        {
            var y = _fontHeight * lineIndex;
            var x = 0;

            x += LEFT_MARGIN;

            var offset = lineIndex * BYTES_PER_LINE;
            var offsetString = offset.ToString("x8");

            g.DrawString(offsetString, _font, _offsetBrush, x, y);
            var measure = g.MeasureString(offsetString, _font);
            //var dx = (int)measure.Width;
            var dx = offsetString.Length * _fontWidth;
            x += dx;

            //var dx1 = (int) g.MeasureString(offsetString, _font).Width;
            var dx1 = TextRenderer.MeasureText(offsetString, _font);

            var dx2 = offsetString.Length * _fontWidth;

            var dd = 33;
            var ddd = g.PageUnit;

            var byteCount = BYTES_PER_LINE;
            if (lineIndex == this.GetLineCount() - 1)
            {
                byteCount = _bytes.Length % BYTES_PER_LINE;
                if (byteCount == 0)
                {
                    byteCount = BYTES_PER_LINE;
                }
            }

            for (int i = 0; i < byteCount; i++)
            {
                var byteIndex = lineIndex * BYTES_PER_LINE + i;
                var b = _bytes[byteIndex];
                var hex = b.ToString("x2");

                g.DrawString(hex, _font, _offsetBrush, x, y);

                x += _fontWidth * 2;
            }

            int k = 3;

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

            var fw2 = TextRenderer.MeasureText("M", _font);

            var waaat = 33;

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

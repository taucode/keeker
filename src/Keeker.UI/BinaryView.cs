using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Keeker.UI
{
    public partial class BinaryView : CustomScrollableControl
    {
        #region Constants

        private const int BYTES_PER_LINE = 16;

        #region Font Names

        private const string DEFAULT_MONOSPACE_FONT_FAMILY_NAME = "Consolas";
        private const string RESERVE_MONOSPACE_FONT_FAMILY_NAME = "Courier New";

        #endregion

        #region Margins

        private const int DEFAULT_ADDRESS_MARGIN = 0;
        private const int DEFAULT_LINE_MARGIN = 16;
        private const int DEFAULT_HEX_MARGIN = 8;
        private const int DEFAULT_HALF_LINE_MARGIN = 16;
        private const int DEFAULT_DUMP_BYTE_MARGIN = 16;

        #endregion

        #region Substitution Strings

        private const string SYMBOL_SUBSTITUTIONS_0x00 = " ☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼";

        private const string SYMBOL_SUBSTITUTIONS_0x7f =
            "⌂АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмноп░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀рстуфхцчшщъыьэюяЁёЄєЇїЎў°∙·√№¤■ ";


        #endregion

        #endregion

        #region Fields

        #region Margins

        private int _addressMargin;
        private int _lineMargin;
        private int _hexMargin;
        private int _halfLineMargin;
        private int _dumpByteMargin;

        #endregion

        private byte[] _bytes;

        private Font _font;

        private int _fontWidth;
        private int _fontHeight;

        private int _baseLineIndex;
        private int _pictureHeight;


        private int _addressPosition;
        private readonly int[] _hexPositions;
        private readonly int[] _dumpBytePositions;

        #endregion

        #region Constructor

        public BinaryView()
        {
            InitializeComponent();

            _addressMargin = DEFAULT_ADDRESS_MARGIN;
            _lineMargin = DEFAULT_LINE_MARGIN;
            _hexMargin = DEFAULT_HEX_MARGIN;
            _halfLineMargin = DEFAULT_HALF_LINE_MARGIN;
            _dumpByteMargin = DEFAULT_DUMP_BYTE_MARGIN;

            var font = this.CreateStandardMonospaceFont();
            this.SetFont(font);

            _bytes = new byte[0];

            _hexPositions = new int[BYTES_PER_LINE];
            _dumpBytePositions = new int[BYTES_PER_LINE];

            this.CalculatePositions();
        }

        #endregion

        #region Overridden

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
            if (lineCount == 0)
            {
                return; // nothing to paint
            }

            var visibleLineCount = this.GetVisibleLineCount();
            if (visibleLineCount == 0)
            {
                return; // nothing to paint
            }


            var fromLineIndex = _baseLineIndex;
            int toLineIndex;

            if (fromLineIndex >= lineCount)
            {
                fromLineIndex = lineCount - 1;
                toLineIndex = fromLineIndex;
            }
            else
            {
                toLineIndex = fromLineIndex + visibleLineCount - 1;
                if (toLineIndex >= lineCount)
                {
                    toLineIndex = lineCount - 1;
                }
            }

            for (var lineIndex = fromLineIndex; lineIndex <= toLineIndex; lineIndex++)
            {
                this.DrawLine(e.Graphics, lineIndex);
            }

            base.OnPaint(e);
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);

            if (se.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                _baseLineIndex = this.VerticalScroll.Value;
                this.Invalidate();
            }
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            this.RecalculateSizes();
            this.RefreshScrollBars();

            base.OnClientSizeChanged(e);
        }

        #endregion

        #region Public Properties

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

        [Browsable(false)]
        public int AddressMargin
        {
            get => _addressMargin;
            set => this.SetMargin(ref _addressMargin, value);
        }

        [Browsable(false)]
        public int LineMargin
        {
            get => _lineMargin;
            set => this.SetMargin(ref _lineMargin, value);
        }

        [Browsable(false)]
        public int HexMargin
        {
            get => _hexMargin;
            set => this.SetMargin(ref _hexMargin, value);
        }

        [Browsable(false)]
        public int HalfLineMargin
        {
            get => _halfLineMargin;
            set => this.SetMargin(ref _halfLineMargin, value);
        }

        [Browsable(false)]
        public int DumpByteMargin
        {
            get => _dumpByteMargin;
            set => this.SetMargin(ref _dumpByteMargin, value);
        }



        #endregion

        #region Private

        private void RenderText(Graphics g, string text, int x, int y, Color color)
        {
            for (int i = 0; i < text.Length; i++)
            {
                var symbolString = text[i].ToString();
                var symbolX = x + _fontWidth * i;
                var pt = new Point(symbolX, y);
                TextRenderer.DrawText(g, symbolString, _font, pt, color);
            }
        }

        private char SubstituteChar(byte b)
        {
            var c = (char)b;

            if (c < 0x20)
            {
                c = SYMBOL_SUBSTITUTIONS_0x00[c];
            }
            else if (c >= 0x7f)
            {
                c = SYMBOL_SUBSTITUTIONS_0x7f[c - 0x7f];
            }

            return c;
        }

        private void DrawLine(Graphics g, int lineIndex)
        {
            var y = _fontHeight * lineIndex;
            var verticalOffset = _baseLineIndex * _fontHeight;
            y -= verticalOffset;

            // render address
            var address = lineIndex * BYTES_PER_LINE;
            var addressString = address.ToString("x8");

            this.RenderText(g, addressString, _addressPosition, y, Color.Black);

            // count number of bytes to render
            var byteCount = BYTES_PER_LINE;
            if (lineIndex == this.GetLineCount() - 1)
            {
                byteCount = _bytes.Length % BYTES_PER_LINE;
                if (byteCount == 0)
                {
                    byteCount = BYTES_PER_LINE;
                }
            }

            // render hex-es & dump bytes
            for (var i = 0; i < byteCount; i++)
            {
                var byteIndex = lineIndex * BYTES_PER_LINE + i;
                var b = _bytes[byteIndex];
                var hex = b.ToString("x2");

                this.RenderText(g, hex, _hexPositions[i], y, Color.Black);

                var c = this.SubstituteChar(b);
                this.RenderText(g, c.ToString(), _dumpBytePositions[i], y, Color.Black);
            }
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

        private int GetVisibleLineCount()
        {
            var count = this.ClientSize.Height / _fontHeight;
            if (this.ClientSize.Height % _fontHeight > 0)
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

        private void SetMargin(ref int margin, int marginValue)
        {
            if (marginValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(margin));
            }

            margin = marginValue;
            this.CalculatePositions();
            this.Invalidate();
        }

        private void CalculatePositions()
        {
            var x = 0;
            var half = BYTES_PER_LINE / 2;

            // address
            x += this.AddressMargin;
            _addressPosition = x;

            x += _fontWidth * 8; // address is 8 bytes long

            // hex-es
            for (var i = 0; i < BYTES_PER_LINE; i++)
            {
                int delta;
                if (i == 0)
                {
                    delta = this.LineMargin;
                }
                else if (i == half)
                {
                    delta = this.HalfLineMargin;
                }
                else
                {
                    delta = this.HexMargin;
                }

                x += delta;
                _hexPositions[i] = x;

                x += _fontWidth * 2;
            }

            x += this.DumpByteMargin;

            // dump bytes
            for (int i = 0; i < BYTES_PER_LINE; i++)
            {
                _dumpBytePositions[i] = x;
                x += _fontWidth;
            }
        }

        private void RecalculateSizes()
        {
            var count = this.GetLineCount();

            _pictureHeight = count * _fontHeight;
        }

        private void RefreshScrollBars()
        {
            var clientHeight = this.ClientSize.Height;

            if (_pictureHeight > clientHeight && clientHeight > 0)
            {
                var defect = _pictureHeight - clientHeight;

                var n = defect / _fontHeight + 1;
                var max = n + 9;
                if (max < 10)
                {
                    max = 10;
                }

                this.VerticalScroll.Maximum = max;
                this.VerticalScroll.Visible = true;
            }
            else
            {
                _baseLineIndex = 0;
                this.VerticalScroll.Value = 0;
                this.VerticalScroll.Visible = false;
            }
        }

        #endregion
    }
}

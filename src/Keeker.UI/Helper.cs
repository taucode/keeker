using System.Drawing;
using System.Windows.Forms;

namespace Keeker.UI
{
    internal static class Helper
    {
        internal static bool IsMonospace(this Font font)
        {
            SizeF sizeM = font.GetCharSize('M');
            SizeF sizeDot = font.GetCharSize('.');
            return sizeM.Equals(sizeDot);
        }

        internal static SizeF GetCharSize(this Font font, char c)
        {
            Size sz2 = TextRenderer.MeasureText("<" + c.ToString() + ">", font);
            Size sz3 = TextRenderer.MeasureText("<>", font);

            return new SizeF(sz2.Width - sz3.Width, /*sz2.Height*/font.Height);
        }
    }
}

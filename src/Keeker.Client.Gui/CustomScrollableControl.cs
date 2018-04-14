using System;
using System.Drawing;
using System.Windows.Forms;

namespace Keeker.Client.Gui
{
    public partial class CustomScrollableControl : UserControl
    {
        #region WIN API Constants

        private const int SB_LINEUP = 0;
        private const int SB_LINEDOWN = 1;
        private const int SB_PAGEUP = 2;
        private const int SB_PAGEDOWN = 3;
        private const int SB_THUMBPOSITION = 4;
        private const int SB_THUMBTRACK = 5;
        private const int SB_TOP = 6;
        private const int SB_BOTTOM = 7;
        private const int SB_ENDSCROLL = 8;

        const int WM_HSCROLL = 0x114;
        const int WM_VSCROLL = 0x115;

        #endregion



        public CustomScrollableControl()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            InitializeComponent();

        }

        

        protected override void WndProc(ref Message m)
        {
            if (m.HWnd == this.Handle)
            {
                switch (m.Msg)
                {
                    case WM_VSCROLL:
                    {
                        if (m.LParam != IntPtr.Zero)
                        {
                            break; //do the base.WndProc
                        }

                        var type = GetScrollEventType(m.WParam);
                        var args = this.BuildScrollEventArgs(this.VerticalScroll, type, m.WParam, ScrollOrientation.VerticalScroll);
                        if (args != null)
                        {
                            this.OnScroll(args);
                            this.Invalidate();
                        }

                        return;
                    }


                    case WM_HSCROLL:
                    {
                        if (m.LParam != IntPtr.Zero)
                        {
                            break; //do the base.WndProc
                        }

                        var type = GetScrollEventType(m.WParam);
                        var args = this.BuildScrollEventArgs(this.HorizontalScroll, type, m.WParam, ScrollOrientation.HorizontalScroll);
                        if (args != null)
                        {
                            this.OnScroll(args);
                            this.Invalidate();
                        }

                        return;
                    }
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!this.VerticalScroll.Visible)
            {
                return;
            }

            var scrollEventType = e.Delta < 0 ? ScrollEventType.SmallIncrement : ScrollEventType.SmallDecrement;

            var args = this.BuildScrollEventArgs(
                this.VerticalScroll,
                scrollEventType,
                IntPtr.Zero,
                ScrollOrientation.VerticalScroll);

            if (args != null)
            {
                this.OnScroll(args);
                this.Invalidate();
            }

            base.OnMouseWheel(e);
        }

        private ScrollEventArgs BuildScrollEventArgs(
            ScrollProperties scrollProperties,
            ScrollEventType scrollEventType,
            IntPtr wParam,
            ScrollOrientation scrollOrientation)
        {
            var oldPos = scrollProperties.Value;
            int? delta = null;
            int? exactPos = null;

            switch (scrollEventType)
            {
                case ScrollEventType.SmallDecrement:
                    delta = -scrollProperties.SmallChange;
                    break;

                case ScrollEventType.SmallIncrement:
                    delta = scrollProperties.SmallChange;
                    break;

                case ScrollEventType.LargeDecrement:
                    delta = -scrollProperties.LargeChange;
                    break;

                case ScrollEventType.LargeIncrement:
                    delta = scrollProperties.LargeChange;
                    break;

                case ScrollEventType.ThumbPosition:
                    exactPos = HiWord((int)wParam);
                    break;

                case ScrollEventType.ThumbTrack:
                    exactPos = HiWord((int)wParam);
                    break;

                case ScrollEventType.First:
                    exactPos = scrollProperties.Minimum;
                    break;

                case ScrollEventType.Last:
                    exactPos = scrollProperties.Maximum;
                    break;
            }

            if (!delta.HasValue && !exactPos.HasValue)
            {
                return null;
            }

            var newPos = oldPos;

            if (delta.HasValue)
            {
                newPos = oldPos + delta.Value;
            }

            if (exactPos.HasValue)
            {
                newPos = exactPos.Value;
            }

            if (newPos < scrollProperties.Minimum)
            {
                newPos = scrollProperties.Minimum;
            }

            if (newPos > scrollProperties.Maximum)
            {
                newPos = scrollProperties.Maximum;
            }

            if (oldPos == newPos)
            {
                return null;
            }
            else
            {
                if (newPos > scrollProperties.Maximum - 9)
                {
                    newPos = scrollProperties.Maximum - 9;
                }

                scrollProperties.Value = newPos;
                var args = new ScrollEventArgs(scrollEventType, oldPos, newPos, scrollOrientation);
                return args;
            }
        }
        
        private static int HiWord(int number)
        {
            if ((number & 0x80000000) == 0x80000000)
                return (number >> 16);
            else
                return (number >> 16) & 0xffff;
        }

        private static int LoWord(int number)
        {
            return number & 0xffff;
        }

        private static ScrollEventType GetScrollEventType(IntPtr wParam)
        {
            ScrollEventType result = 0;
            switch (LoWord((int)wParam))
            {
                case SB_LINEUP:
                    result = ScrollEventType.SmallDecrement;
                    break;
                case SB_LINEDOWN:
                    result = ScrollEventType.SmallIncrement;
                    break;
                case SB_PAGEUP:
                    result = ScrollEventType.LargeDecrement;
                    break;
                case SB_PAGEDOWN:
                    result = ScrollEventType.LargeIncrement;
                    break;
                case SB_THUMBTRACK:
                    result = ScrollEventType.ThumbTrack;
                    break;
                case SB_TOP:
                    result = ScrollEventType.First;
                    break;
                case SB_BOTTOM:
                    result = ScrollEventType.Last;
                    break;
                case SB_THUMBPOSITION:
                    result = ScrollEventType.ThumbPosition;
                    break;
                case SB_ENDSCROLL:
                    result = ScrollEventType.EndScroll;
                    break;
                default:
                    result = ScrollEventType.EndScroll;
                    break;
            }
            return result;
        }

        
    }
}

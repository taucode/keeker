using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Keeker.Gui.Controls
{
    public partial class RaceChart : UserControl
    {
        private const int SB_LINEUP = 0;
        private const int SB_LINEDOWN = 1;
        private const int SB_PAGEUP = 2;
        private const int SB_PAGEDOWN = 3;
        private const int SB_THUMBPOSITION = 4;
        private const int SB_THUMBTRACK = 5;
        private const int SB_TOP = 6;
        private const int SB_BOTTOM = 7;
        private const int SB_ENDSCROLL = 8;

        private const int WHEEL_DELTA = 120;

        public RaceChart()
        {
            InitializeComponent();

            this.VScroll = true;
            this.HScroll = true;

            this.MouseWheel += RaceChart_MouseWheel;

            this.Participants = new RaceChartParticipantCollection();
        }

        private void RaceChart_MouseWheel(object sender, MouseEventArgs e)
        {
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
        }

        [Browsable(false)]
        public Func<RaceChartEntry, RaceChartEntry, bool> EntryComparer;

        [Browsable(false)]
        public RaceChartParticipantCollection Participants { get; }

        protected override void OnLoad(EventArgs e)
        {
            this.VerticalScroll.Visible = true;
            this.HorizontalScroll.Visible = true;

            base.OnLoad(e);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HSCROLL = 0x114;
            const int WM_VSCROLL = 0x115;
            const int WM_MOUSEWHEEL = 0x020A;

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
                scrollProperties.Value = newPos;
                var args = new ScrollEventArgs(scrollEventType, newPos, oldPos, scrollOrientation);
                return args;
            }
        }

        private ScrollEventType GetScrollEventType(IntPtr wParam)
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

        static int HiWord(int number)
        {
            if ((number & 0x80000000) == 0x80000000)
                return (number >> 16);
            else
                return (number >> 16) & 0xffff;
        }

        static int LoWord(int number)
        {
            return number & 0xffff;
        }
    }
}

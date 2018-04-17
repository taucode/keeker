using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Keeker.Gui.Controls
{
    public partial class RaceChart : UserControl
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

        #region Drawing Constants

        const int ENTRY_WIDTH = 60;
        const int ENTRY_HEIGHT = 20;

        const int VERT_MARGIN = 2;
        const int HORZ_MARGIN = 2;

        const int VERT_SPACING = 2;
        const int HORZ_SPACING = 8;

        #endregion

        #region Fields

        private List<List<RaceChartEntry>> _entryCollections;
        private List<RaceChartEntry> _allEntries;

        private int _base;


        private readonly int _itemHeightWithSpacing;
        private readonly int _scrollStep;
        private int _pictureHeight;

        #endregion

        #region Constructor

        public RaceChart()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            InitializeComponent();

            this.VScroll = true;
            this.HScroll = true;

            _itemHeightWithSpacing = ENTRY_HEIGHT + VERT_SPACING;
            _scrollStep = _itemHeightWithSpacing;
        }

        #endregion

        #region Overridden

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);

            if (se.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                _base = this.VerticalScroll.Value * _scrollStep;
                this.Invalidate();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            //this.VerticalScroll.SmallChange = this.GetItemHeightWithSpacing() / 2;

            base.OnLoad(e);
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

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            this.DoPainting(e);
            base.OnPaint(e);
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

        protected override void OnClientSizeChanged(EventArgs e)
        {
            this.RecalculateSizes();
            this.RefreshScrollBars();

            base.OnClientSizeChanged(e);
        }

        #endregion

        #region Public

        public void InitParticipants(int participantCount)
        {
            if (_entryCollections != null)
            {
                throw new ApplicationException(); // todo1[ak]
            }

            // todo1[ak] checks
            _entryCollections = new List<List<RaceChartEntry>>();

            for (var i = 0; i < participantCount; i++)
            {
                var entries = new List<RaceChartEntry>();
                _entryCollections.Add(entries);
            }
        }

        [Browsable(false)]
        public Comparison<RaceChartEntry> EntryComparer;

        public void AddEntry(RaceChartEntry entry, int participantIndex)
        {
            var entries = _entryCollections[participantIndex];
            entries.Add(entry);
            entry.ParticipantIndex = participantIndex;

            _allEntries = this.BuildAllEntries();

            this.RecalculateSizes();
            this.RefreshScrollBars();
            this.Invalidate();
        }


        [Browsable(false)]
        public int ParticipantCount => _entryCollections.Count;

        #endregion

        #region Private

        private void RecalculateSizes()
        {
            var list = _allEntries;
            var count = list?.Count ?? 0;

            _pictureHeight =
                VERT_MARGIN +
                count * _itemHeightWithSpacing;
        }

        private void RefreshScrollBars()
        {
            var clientHeight = this.ClientSize.Height;

            if (_pictureHeight > clientHeight && clientHeight > 0)
            {
                var defect = _pictureHeight - clientHeight;
                var n = defect / _scrollStep + 1;
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
                _base = 0;
                this.VerticalScroll.Value = 0;
                this.VerticalScroll.Visible = false;
            }
        }

        private List<RaceChartEntry> BuildAllEntries()
        {
            var list = new List<RaceChartEntry>();
            foreach (var entryCollection in _entryCollections)
            {
                list.AddRange(entryCollection);
            }

            list.Sort(this.EntryComparer);
            return list;
        }

        private void DoPainting(PaintEventArgs e)
        {
            var g = e.Graphics;
            var list = _allEntries;

            var clientPen = new Pen(Color.Green);
            var itemPen = new Pen(Color.Black);

            //g.DrawRectangle(clientPen, 0, 0, this.ClientSize.Width - 2, this.ClientSize.Height - 2);

            if (list == null)
            {
                _base = 0;
                this.VerticalScroll.Value = 0;
                this.VerticalScroll.Visible = false;
            }
            else
            {
                for (var i = 0; i < list.Count; i++)
                {
                    var entry = list[i];

                    var idx = entry.ParticipantIndex;
                    var x = HORZ_MARGIN + idx * (ENTRY_WIDTH + HORZ_SPACING);
                    var y = VERT_MARGIN + i * (ENTRY_HEIGHT + VERT_SPACING);

                    y -= _base;

                    g.DrawRectangle(itemPen, x, y, ENTRY_WIDTH, ENTRY_HEIGHT);
                }

                //var clientHeight = this.ClientSize.Height;
                //var p = this.GetPictureHeight();

                //if (p > clientHeight && clientHeight > 0)
                //{
                //    var defect = p - clientHeight;
                //    var n = defect / _step + 1;
                //    var max = n + 9;
                //    if (max < 10)
                //    {
                //        max = 10;
                //    }

                //    this.VerticalScroll.Maximum = max;
                //    this.VerticalScroll.Visible = true;
                //}
                //else
                //{
                //    _base = 0;
                //    this.VerticalScroll.Value = 0;
                //    this.VerticalScroll.Visible = false;
                //}
            }

            itemPen.Dispose();
            clientPen.Dispose();
        }

        //private int GetItemHeightWithSpacing()
        //{
        //    return ENTRY_HEIGHT + VERT_SPACING;
        //}

        //private int GetPictureHeight()
        //{
        //    var list = _allEntries;
        //    var count = list?.Count ?? 0;

        //    var pictureHeight =
        //        VERT_MARGIN +
        //        count * _itemHeightWithSpacing;

        //    return pictureHeight;
        //}

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

        #endregion

        #region Private Utility

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

        #endregion
    }
}

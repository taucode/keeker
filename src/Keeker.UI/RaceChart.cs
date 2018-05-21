using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Keeker.UI
{
    public partial class RaceChart : CustomScrollableControl
    {
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

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            this.DoPainting(e);
            base.OnPaint(e);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            this.RecalculateSizes();
            this.RefreshScrollBars();

            base.OnClientSizeChanged(e);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            throw new NotImplementedException();
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
            }

            itemPen.Dispose();
            clientPen.Dispose();
        }

        #endregion
    }
}

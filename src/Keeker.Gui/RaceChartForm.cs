using Keeker.Gui.Controls;
using System;
using System.Windows.Forms;

namespace Keeker.Gui
{
    public partial class RaceChartForm : Form
    {
        //private RaceChartParticipant _client;
        //private RaceChartParticipant _server;

        private int _counter;

        public RaceChartForm()
        {
            InitializeComponent();
        }

        private void RaceChartForm_Load(object sender, System.EventArgs e)
        {
            raceChartMain.InitParticipants(2);
            raceChartMain.EntryComparer = this.CompareEntries;

            for (int i = 0; i < 14; i++)
            {
                raceChartMain.AddEntry(new RaceChartEntry(new FooData(++_counter)), i % 2);
            }

            raceChartMain.Invalidate();
        }

        private int CompareEntries(RaceChartEntry x, RaceChartEntry y)
        {
            var fooX = (FooData)x.Data;
            var fooY = (FooData)y.Data;

            return fooX.When.CompareTo(fooY.When);
        }

        private void buttonClient_Click(object sender, EventArgs e)
        {
            raceChartMain.AddEntry(new RaceChartEntry(new FooData(++_counter)), 0);
        }

        private void buttonServer_Click(object sender, EventArgs e)
        {
            raceChartMain.AddEntry(new RaceChartEntry(new FooData(++_counter)), 1);
        }

        private void buttonGet_Click(object sender, EventArgs e)
        {
            try
            {
                var props = raceChartMain.VerticalScroll;
                textBoxMin.Text = props.Minimum.ToString();
                textBoxMax.Text = props.Maximum.ToString();
                textBoxPos.Text = props.Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            try
            {
                throw new NotImplementedException();
                //var props = raceChartMain.VerticalScroll;
                //props.Minimum = textBoxMin.Text.ToInt32();
                //props.Maximum = textBoxMax.Text.ToInt32();
                //props.Value = textBoxPos.Text.ToInt32();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonToggle_Click(object sender, EventArgs e)
        {
            try
            {
                raceChartMain.VerticalScroll.Visible = !raceChartMain.VerticalScroll.Visible;

                buttonGet_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void raceChartMain_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                buttonGet_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

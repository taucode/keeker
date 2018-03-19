namespace Keeker.Gui.Controls
{
    public class RaceChartEntry
    {
        public RaceChartEntry()
        {
        }

        public RaceChartEntry(object data)
        {
            this.Data = data;
        }

        public object Data { get; set; }

        internal int ParticipantIndex { get; set; }
    }
}

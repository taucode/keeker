namespace Keeker.UI
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

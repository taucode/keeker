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

        public object Data { get; private set; }

        public int ParticipantIndex { get; internal set; }

        public int Index { get; internal set; }
    }
}

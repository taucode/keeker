namespace Keeker.Gui.Panes
{
    public partial class RelayPane : Pane
    {
        public RelayPane()
        {
            InitializeComponent();
        }

        //public RelayPane(IRelay relay)
        //    : this()
        //{
        //    this.Relay = relay;
        //}

        //public IRelay Relay { get; }

        private void RelayPane_Load(object sender, System.EventArgs e)
        {
            raceChartPackets.InitParticipants(2);
        }
    }
}

using Keeker.Core.Relays;

namespace Keeker.Gui.Panes
{
    public partial class RelayPane : Pane
    {
        public RelayPane()
        {
            InitializeComponent();
        }

        //public RelayPane(Relay relay)
        //{
        //    this.Relay = relay;

            
        //}

        //public Relay Relay { get; }

        private void RelayPane_Load(object sender, System.EventArgs e)
        {
            raceChartPackets.InitParticipants(2);
        }
    }
}

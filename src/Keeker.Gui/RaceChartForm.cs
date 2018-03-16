using Keeker.Gui.Controls;
using System.Windows.Forms;

namespace Keeker.Gui
{
    public partial class RaceChartForm : Form
    {
        private RaceChartParticipant _client;
        private RaceChartParticipant _server;

        public RaceChartForm()
        {
            InitializeComponent();

            _client = new RaceChartParticipant();
            _server = new RaceChartParticipant();

        }
    }
}

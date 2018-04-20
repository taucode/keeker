using Keeker.Test.DemoFirst;
using Keeker.Test.DemoSecond;
using Keeker.UI;
using Keeker.Utils;
using Serilog;
using Serilog.Context;
using System.Windows.Forms;

namespace Keeker.Test.Gui
{
    public partial class MainForm : Form
    {
        private readonly LogForm _logFormFirst;
        private readonly LogForm _logFormSecond;

        private readonly FirstComponent _first;
        private readonly SecondComponent _second;

        public MainForm()
        {
            InitializeComponent();

            _logFormFirst = new LogForm
            {
                Text = "First",
            };
            
            _logFormSecond = new LogForm
            {
                Text = "Second",
            };

            var logFirst = new LoggerConfiguration()
                .WriteTo.TextWriter(_logFormFirst.TextWriter, outputTemplate: "{Timestamp:HH:mm} [{Level}] ({Name:l}) {Message}{NewLine}{Exception}")
                .CreateLogger();
            FirstHelper.SetLogger(
                name => new SerilogDebugWrapper<Keeker.Test.DemoFirst.Logging.LogLevel>(logFirst).Log, 
                message => LogContext.PushProperty("NDC", message),
                (key, value) => LogContext.PushProperty(key, value, false));

            var logSecond = new LoggerConfiguration()
                .WriteTo.TextWriter(_logFormSecond.TextWriter, outputTemplate: "{Timestamp:HH:mm} [{Level}] ({Name:l}) {Message}{NewLine}{Exception}")
                .CreateLogger();
            SecondHelper.SetLogger(
                name => new SerilogDebugWrapper<Keeker.Test.DemoSecond.Logging.LogLevel>(logSecond).Log,
                message => LogContext.PushProperty("NDC", message),
                (key, value) => LogContext.PushProperty(key, value, false));

            _first = new FirstComponent();
            _second = new SecondComponent();
        }

        private void buttonFirstComponent_Click(object sender, System.EventArgs e)
        {
            _first.DoFirst();
        }

        private void buttonSecondComponent_Click(object sender, System.EventArgs e)
        {
            _second.DoSecond();
        }

        private void buttonFirstLog_Click(object sender, System.EventArgs e)
        {
            _logFormFirst.Show();
        }

        private void buttonSecondLog_Click(object sender, System.EventArgs e)
        {
            _logFormSecond.Show();
        }
    }
}

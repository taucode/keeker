using Keeker.Test.DemoFirst.Logging;

namespace Keeker.Test.DemoFirst
{
    public class FirstComponent
    {
        private static ILog _logger = LogProvider.GetCurrentClassLogger();

        public static void SetSerilog(Serilog.Core.Logger serilogLogger)
        {
            _logger = new LoggerExecutionWrapper(new CustomSerilogLogProvider(serilogLogger).GetLogger("First"));
        }

        public void DoFirst()
        {
            _logger.Info("First says hello!");
        }
    }
}

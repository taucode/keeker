using Keeker.Test.DemoSecond.Logging;

namespace Keeker.Test.DemoSecond
{
    public class SecondComponent
    {
        private static ILog _logger = LogProvider.GetCurrentClassLogger();

        public static void SetSerilog(Serilog.Core.Logger serilogLogger)
        {
            _logger = new LoggerExecutionWrapper(new CustomSerilogLogProvider(serilogLogger).GetLogger("Second"));
        }

        public void DoSecond()
        {
            _logger.Info("Second says hello!");
        }
    }
}

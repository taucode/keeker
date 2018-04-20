using Serilog;
using Serilog.Events;
using System;

namespace Keeker.Utils
{
    public class SerilogDebugWrapper<TLogLevel> where TLogLevel : struct
    {
        private readonly ILogger _logger;
        

        public SerilogDebugWrapper(ILogger logger)
        {
            _logger = logger;

            logger.ForContext("SourceContext", "name", destructureObjects: false);
        }

        public bool Log(TLogLevel logLevel, Func<string> messageFunc, Exception exception, params object[] formatParameters)
        {
            var translatedLevel = TranslateLevel(logLevel);
            if (messageFunc == null)
            {
                return _logger.IsEnabled(translatedLevel);
            }

            if (!_logger.IsEnabled(translatedLevel))
            {
                return false;
            }

            if (exception != null)
            {
                LogException(translatedLevel, messageFunc, exception, formatParameters);
            }
            else
            {
                LogMessage(translatedLevel, messageFunc, formatParameters);
            }

            return true;
        }

        private void LogMessage(LogEventLevel logLevel, Func<string> messageFunc, object[] formatParameters)
        {
            _logger.Write(logLevel, messageFunc(), formatParameters);
        }

        private void LogException(LogEventLevel logLevel, Func<string> messageFunc, Exception exception, object[] formatParams)
        {
            _logger.Write(logLevel, exception, messageFunc(), formatParams);
        }

        private static LogEventLevel TranslateLevel(TLogLevel logLevel)
        {
            switch (logLevel.ToString())
            {
                case "Fatal":
                    return Serilog.Events.LogEventLevel.Fatal;
                case "Error":
                    return Serilog.Events.LogEventLevel.Error;
                case "Warn":
                    return Serilog.Events.LogEventLevel.Warning;
                case "Info":
                    return Serilog.Events.LogEventLevel.Information;
                case "Trace":
                    return Serilog.Events.LogEventLevel.Verbose;
                case "Debug":
                    return Serilog.Events.LogEventLevel.Debug;
                default:
                    return Serilog.Events.LogEventLevel.Debug;
            }
        }

    }
}

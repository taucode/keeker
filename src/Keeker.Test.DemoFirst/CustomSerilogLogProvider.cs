﻿using Keeker.Test.DemoFirst.Logging;
using Serilog;
using Serilog.Context;
using System;

namespace Keeker.Test.DemoFirst
{
    internal class CustomSerilogLogProvider : ILogProvider
    {
        public ILogger Logger { get; set; }

        public CustomSerilogLogProvider(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            Logger = logger;
        }

        public Logger GetLogger(string name)
        {
            return new SerilogLoggerWrapper(Logger.ForContext("SourceContext", name, destructureObjects: false)).Log;
        }

        private object ForContext(string name)
        {
            return Logger.ForContext("SourceContext", name, destructureObjects: false);
        }

        public IDisposable OpenNestedContext(string message)
        {
            return LogContext.PushProperty("NDC", message);
        }

        public IDisposable OpenMappedContext(string key, string value)
        {
            return LogContext.PushProperty(key, value, false);
        }

        internal class SerilogLoggerWrapper
        {
            private ILogger logger;

            public SerilogLoggerWrapper(ILogger logger)
            {
                this.logger = logger;
            }

            public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception, params object[] formatParameters)
            {
                var translatedLevel = TranslateLevel(logLevel);
                if (messageFunc == null)
                {
                    return logger.IsEnabled(translatedLevel);
                }

                if (!logger.IsEnabled(translatedLevel))
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

            private void LogMessage(Serilog.Events.LogEventLevel logLevel, Func<string> messageFunc, object[] formatParameters)
            {
                logger.Write(logLevel, messageFunc(), formatParameters);
            }

            private void LogException(Serilog.Events.LogEventLevel logLevel, Func<string> messageFunc, Exception exception, object[] formatParams)
            {
                logger.Write(logLevel, exception, messageFunc(), formatParams);
            }

            private static Serilog.Events.LogEventLevel TranslateLevel(LogLevel logLevel)
            {
                switch (logLevel)
                {
                    case LogLevel.Fatal:
                        return Serilog.Events.LogEventLevel.Fatal;
                    case LogLevel.Error:
                        return Serilog.Events.LogEventLevel.Error;
                    case LogLevel.Warn:
                        return Serilog.Events.LogEventLevel.Warning;
                    case LogLevel.Info:
                        return Serilog.Events.LogEventLevel.Information;
                    case LogLevel.Trace:
                        return Serilog.Events.LogEventLevel.Verbose;
                    default:
                        return Serilog.Events.LogEventLevel.Debug;
                }
            }
        }
    }
}
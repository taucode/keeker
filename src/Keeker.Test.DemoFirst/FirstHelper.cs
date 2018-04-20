using Keeker.Test.DemoFirst.Logging;
using System;

namespace Keeker.Test.DemoFirst
{
    public static class FirstHelper
    {
        private class LogProviderImpl : ILogProvider
        {
            private readonly Func<string, Logger> _loggerResolver;
            private readonly Func<string, IDisposable> _openNestedContext;
            private readonly Func<string, string, IDisposable> _openMappedContext;

            public LogProviderImpl(
                Func<string, Logger> loggerResolver,
                Func<string, IDisposable> openNestedContext,
                Func<string, string, IDisposable> openMappedContext)
            {
                _loggerResolver = loggerResolver;
                _openNestedContext = openNestedContext;
                _openMappedContext = openMappedContext;
            }

            public Logger GetLogger(string name)
            {
                return _loggerResolver(name);
            }

            public IDisposable OpenNestedContext(string message)
            {
                return _openNestedContext(message);
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                return _openMappedContext(key, value);
            }
        }

        internal static ILog _logger;

        static FirstHelper()
        {
            _logger = LogProvider.GetCurrentClassLogger();
        }

        internal static ILog GetLogger()
        {
            return _logger;
        }

        public static void SetLogger(
            Func<string, Logger> loggerResolver,
            Func<string, IDisposable> openNestedContext,
            Func<string, string, IDisposable> openMappedContext)
        {
            var provider = new LogProviderImpl(loggerResolver, openNestedContext, openMappedContext);
            _logger = new LoggerExecutionWrapper(provider.GetLogger(""));
        }
    }
}

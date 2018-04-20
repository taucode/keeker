//using Keeker.Server.Logging;
//using System;

//namespace Keeker.Server
//{
//    public static class ServerHelper
//    {
//        //private static readonly ILog DefaultLog = LogProvider.GetCurrentClassLogger();
//        private static ILog _currentLog;

//        private class LogImpl : ILog
//        {
//            private readonly Logger _logger;

//            public LogImpl(Logger logger)
//            {
//                _logger = logger;
//            }

//            public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
//            {
//                return _logger(logLevel, messageFunc, exception, formatParameters);
//            }
//        }

//        static ServerHelper()
//        {
//            _currentLog = LogProvider.GetCurrentClassLogger();
//        }

//        internal static ILog GetLogger()
//        {
//            return _currentLog;
//        }

//        public static void SetLogger(Logger logger)
//        {
//            var wat = new CustomSerilogLogProvider();
//            wat.GetLogger();

//        }
//    }
//}

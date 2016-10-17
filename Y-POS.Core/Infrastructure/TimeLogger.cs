using System;
using System.Diagnostics;
using YumaPos.Common.Infrastructure.Logging;

namespace Y_POS.Core.Infrastructure
{
    public static class TimeLogger
    {
        public static ILog logger;

        public static IDisposable GetTimeLogger(string methodName)
        {
            return new TimeLoggerInstance(logger, methodName);
        }

        private class TimeLoggerInstance : IDisposable
        {
            private readonly Stopwatch _sw = new Stopwatch();
            private readonly string _methodName;
            private readonly ILog _logger;

            public TimeLoggerInstance(ILog logger, string methodName)
            {
                _logger = logger;
                _methodName = methodName;
                _sw.Start();
            }

            public void Dispose()
            {
                _logger.Info($"Method {_methodName} took {_sw.ElapsedMilliseconds}ms");
                _sw.Stop();
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace Ack.Infrastructure.Loggers
{
    public class AggreagateLogger : ILogger
    {
        private IEnumerable<ILogger> Loggers { get; set; }

        public AggreagateLogger(IEnumerable<ILogger> loggers)
        {
            if (loggers == null)
            {
                throw new ArgumentNullException("loggers");
            }

            Loggers = loggers;
        }

        public void Log(string message)
        {
            foreach (var logger in Loggers)
            {
                try
                {
                    logger.Log(message);
                }
                catch { }
            }
        }
    }
}

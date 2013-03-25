using System;

namespace Ack.Infrastructure.Loggers.Console
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            System.Console.WriteLine("{0}: {1}", DateTime.UtcNow, message);
        }
    }
}

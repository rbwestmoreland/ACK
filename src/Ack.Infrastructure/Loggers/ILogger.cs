using System;

namespace Ack.Infrastructure.Loggers
{
    public interface ILogger
    {
        void Log(string message);
    }
}

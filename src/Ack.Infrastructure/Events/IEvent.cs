using System;

namespace Ack.Infrastructure.Events
{
    public interface IEvent
    {
        string Type { get; }
        DateTime Timestamp { get; }
        string Data { get; }
    }
}

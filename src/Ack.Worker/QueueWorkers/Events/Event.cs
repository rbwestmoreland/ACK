using Ack.Infrastructure.Events;
using System;

namespace Ack.Worker.QueueWorkers.Events
{
    public class Event : IEvent
    {
        public string Type { get; set; }
        public DateTime Timestamp { get; set; }
        public string Data { get; set; }
    }
}

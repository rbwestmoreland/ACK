using System;

namespace Ack.Infrastructure.Queues
{
    internal class QueueMessage : IQueueMessage
    {
        public string Id { get; set; }
        public string Data { get; set; }
    }
}

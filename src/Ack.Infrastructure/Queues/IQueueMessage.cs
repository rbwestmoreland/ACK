using System;

namespace Ack.Infrastructure.Queues
{
    public interface IQueueMessage
    {
        string Id { get; }
        string Data { get; }
    }
}

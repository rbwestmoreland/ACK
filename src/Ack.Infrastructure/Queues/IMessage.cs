using System;

namespace Ack.Infrastructure.Queues
{
    public interface IMessage
    {
        string Id { get; }
        string Data { get; }
    }
}

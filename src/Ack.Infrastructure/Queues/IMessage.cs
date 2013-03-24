using System;

namespace Ack.Infrastructure.Queues
{
    public interface IMessage
    {
        string Data { get; }
    }
}

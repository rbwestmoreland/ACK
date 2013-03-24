using System;

namespace Ack.Infrastructure.Queues
{
    public class Message : IMessage
    {
        public string Data { get; set; }
    }
}

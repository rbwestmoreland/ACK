using System;

namespace Ack.Infrastructure.Queues
{
    public class Message : IMessage
    {
        public string Id { get; set; }
        public string Data { get; set; }
    }
}

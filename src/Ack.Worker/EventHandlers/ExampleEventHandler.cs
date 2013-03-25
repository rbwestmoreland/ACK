using Ack.Infrastructure.Events;
using System;

namespace Ack.Worker.EventHandlers
{
    public class ExampleEventHandler : IEventHandler
    {
        private const string Type = "example:v1";

        public void Handle(IEvent @event)
        {
            if (!@event.Type.Equals(Type, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            Console.WriteLine("{0} handled event type {1}", this.GetType(), @event.Type);
        }
    }
}

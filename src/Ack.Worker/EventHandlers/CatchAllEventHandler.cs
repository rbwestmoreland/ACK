using Ack.Infrastructure.Events;
using System;

namespace Ack.Worker.EventHandlers
{
    public class CatchAllEventHandler : IEventHandler
    {
        public void Handle(IEvent @event)
        {
            Console.WriteLine("{0} handled event type {1}", this.GetType(), @event.Type);
        }
    }
}

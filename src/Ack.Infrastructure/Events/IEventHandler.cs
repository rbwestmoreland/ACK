using System;

namespace Ack.Infrastructure.Events
{
    public interface IEventHandler
    {
        void Handle(IEvent @event);
    }
}

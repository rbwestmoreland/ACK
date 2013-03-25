using Ack.Infrastructure.Events;
using Ack.Infrastructure.Loggers;
using Ack.Infrastructure.Queues;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Ack.Worker.QueueWorkers.Events
{
    public class EventQueueWorker : IQueueWorker
    {
        private IQueue Queue { get; set; }
        private IEnumerable<IEventHandler> EventHandlers { get; set; }
        private ILogger Logger { get; set; }

        public EventQueueWorker(IQueue queue,
            IEnumerable<IEventHandler> eventHandlers,
            ILogger logger)
        {
            if (queue == null)
            {
                throw new ArgumentNullException("queue");
            }

            if (eventHandlers == null)
            {
                throw new ArgumentNullException("eventHandlers");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            Queue = queue;
            EventHandlers = eventHandlers;
            Logger = logger;
        }

        public void ProcessNextMessage()
        {
            var queueMessage = Queue.Peek().Result;

            if (queueMessage == null)
            {
                Logger.Log("no messages");
                return;
            }

            Event @event;
            if (!TryDeserializeEvent(queueMessage, out @event))
            {
                Logger.Log("received an invalid event");
                Queue.Pop(queueMessage);
                return;
            }

            var shouldPop = true;

            foreach (var handler in EventHandlers)
            {
                try
                {
                    handler.Handle(@event);
                }
                catch
                {
                    shouldPop = false;
                    Logger.Log(string.Format("handler {0} exited unsuccessfully", handler.GetType()));
                }
            }

            if (shouldPop)
            {
                Queue.Pop(queueMessage).Wait();
                Logger.Log(string.Format("processed message {0}", queueMessage.Id));
            }
        }

        public static bool TryDeserializeEvent(IQueueMessage queueMessage, out Event @event)
        {
            @event = default(Event);

            try
            {
                @event = JsonConvert.DeserializeObject<Event>(queueMessage.Data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

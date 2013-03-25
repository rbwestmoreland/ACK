﻿using Ack.Infrastructure.Events;
using Ack.Infrastructure.Loggers;
using Ack.Infrastructure.Loggers.Console;
using Ack.Infrastructure.Queues;
using Ack.Infrastructure.Queues.IronIo;
using Ack.Worker.EventHandlers;
using Ack.Worker.QueueWorkers;
using Ack.Worker.QueueWorkers.Events;
using TinyIoC;

namespace Ack.Worker
{
    public static class IoCConfig
    {
        public static TinyIoCContainer Container { get; private set; }

        static IoCConfig()
        {
            Container = new TinyIoCContainer();
            RegisterAll();
        }

        private static void RegisterAll()
        {
            var ironIoToken = "";
            var ironIoProjectId = "";
            var ironIoQueueName = "";
            var ironIoQueueUrl = "https://mq-aws-us-east-1.iron.io";

            Container.Register<IQueue>((c, n) => new IronIoQueue(ironIoToken, ironIoProjectId, ironIoQueueName, ironIoQueueUrl));

            Container.Register<IEventHandler, CatchAllEventHandler>("CatchAllEventHandler");
            Container.Register<IEventHandler, ExampleEventHandler>("ExampleEventHandler");

            Container.Register<ILogger, ConsoleLogger>("ConsoleLogger");
            Container.Register<ILogger>((c, n) =>
                {
                    var loggers = c.ResolveAll<ILogger>(false);
                    return new AggreagateLogger(loggers);
                });

            Container.Register<IQueueWorker, EventQueueWorker>();
        }
    }
}

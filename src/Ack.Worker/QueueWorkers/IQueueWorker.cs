using System;

namespace Ack.Worker.QueueWorkers
{
    public interface IQueueWorker
    {
        void ProcessNextMessage();
    }
}

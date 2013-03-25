using Ack.Worker.QueueWorkers;
using System.Threading;

namespace Ack.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var queueWorker = IoCConfig.Container.Resolve<IQueueWorker>();

            while (true)
            {
                queueWorker.ProcessNextMessage();
                Thread.Sleep(1000);
            }
        }
    }
}

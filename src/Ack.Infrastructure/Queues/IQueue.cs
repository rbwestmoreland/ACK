using System.Threading.Tasks;

namespace Ack.Infrastructure.Queues
{
    public interface IQueue
    {
        Task Push(string data);
        Task<IQueueMessage> Peek();
        Task Pop(IQueueMessage queueMessage);
    }
}

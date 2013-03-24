using System.Threading.Tasks;

namespace Ack.Infrastructure.Queues
{
    public interface IQueue
    {
        Task Push(string data);
        Task<IMessage> Peek();
        Task Pop(IMessage message);
    }
}

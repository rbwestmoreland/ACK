using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;

namespace Ack.Infrastructure.Queues.Amqp
{
    public class AmqpQueue : IQueue
    {
        private const string QueueName = "ack";
        private ConnectionFactory ConnectionFactory { get; set; }

        public AmqpQueue(string hostName,
            string username,
            string password,
            string virtualHost,
            int port)
        {
            ConnectionFactory = new ConnectionFactory();
            ConnectionFactory.HostName = hostName;
            ConnectionFactory.UserName = username;
            ConnectionFactory.Password = password;
            ConnectionFactory.VirtualHost = virtualHost;
            ConnectionFactory.Port = port;
        }
        
        public Task Push(string data)
        {
            return Task.Factory.StartNew(() =>
                {
                    using(var connection = ConnectionFactory.CreateConnection())
                    using(var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(QueueName, true, false, false, null);
                        var body = Encoding.UTF8.GetBytes(data);
                        channel.BasicPublish(string.Empty, QueueName, null, body);
                    }
                });
        }

        public Task<IQueueMessage> Peek()
        {
            return Task.Factory.StartNew(() =>
            {
                var message = default(IQueueMessage);

                using (var connection = ConnectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(QueueName, true, false, false, null);
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(QueueName, false, consumer);

                    object result;
                    consumer.Queue.Dequeue(500, out result);

                    var eventArgs = result as BasicDeliverEventArgs;
                    if (eventArgs != null)
                    {
                        var body = System.Text.Encoding.UTF8.GetString(eventArgs.Body);
                        message = new QueueMessage
                        {
                            Id = eventArgs.DeliveryTag.ToString(),
                            Data = body
                        };
                    }

                    return message;
                }
            });
        }

        public Task Pop(IQueueMessage queueMessage)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var connection = ConnectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(QueueName, true, false, false, null);
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(QueueName, false, consumer);

                    ulong id;
                    if (ulong.TryParse(queueMessage.Id, out id))
                    {
                        channel.BasicAck(id, false);
                    }
                }
            });
        }
    }
}

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;

namespace Ack.Infrastructure.Queues.Amqp
{
    public class AmqpQueue : IQueue
    {
        private const string QueueName = "ack";
        private string HostName { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string VirtualHost { get; set; }
        private int Port { get; set; }
        private IModel Channel { get; set; }

        public AmqpQueue(string hostName,
            string username,
            string password,
            string virtualHost,
            int port)
        {
            HostName = hostName;
            Username = username;
            Password = password;
            VirtualHost = virtualHost;
            Port = port;

            Initialize();
        }

        private void Initialize()
        {
            var connectionFactory = new ConnectionFactory
                {
                    HostName = HostName,
                    UserName = Username,
                    Password = Password,
                    VirtualHost = VirtualHost,
                    Port = Port
                };
            //start persistent connection
            var connection = connectionFactory.CreateConnection();
            //create channel
            Channel = connection.CreateModel();
            //create queue
            Channel.QueueDeclare(QueueName, true, false, false, null);
        }

        public Task Push(string data)
        {
            return Task.Factory.StartNew(() =>
                {
                    //encode data
                    var body = Encoding.UTF8.GetBytes(data);
                    //enqueue message
                    Channel.BasicPublish(string.Empty, QueueName, null, body);
                });
        }

        public Task<IQueueMessage> Peek()
        {
            return Task.Factory.StartNew(() =>
            {
                var message = default(IQueueMessage);

                //create queue consumer
                var consumer = new QueueingBasicConsumer(Channel);
                //start queue consumer with acknowledgment
                Channel.BasicConsume(QueueName, false, consumer);

                //dequeue message (not removed until acknowledged)
                object result;
                consumer.Queue.Dequeue(500, out result);

                //null check
                var eventArgs = result as BasicDeliverEventArgs;
                if (eventArgs != null)
                {
                    //decode data
                    var body = Encoding.UTF8.GetString(eventArgs.Body);
                    message = new QueueMessage
                    {
                        Id = eventArgs.DeliveryTag.ToString(),
                        Data = body
                    };
                }

                return message;
            });
        }

        public Task Pop(IQueueMessage queueMessage)
        {
            return Task.Factory.StartNew(() =>
            {
                //acknowledgment message (remove from queue)
                ulong id;
                if (ulong.TryParse(queueMessage.Id, out id))
                {
                    Channel.BasicAck(id, false);
                }
            });
        }
    }
}

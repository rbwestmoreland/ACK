using System;
using System.Messaging;
using System.Threading.Tasks;

namespace Ack.Infrastructure.Queues.Msmq
{
    public class MsmqQueue : IQueue
    {
        private string QueuePath { get; set; }

        public MsmqQueue(string queuePath)
        {
            if (string.IsNullOrWhiteSpace(queuePath))
            {
                throw new ArgumentNullException("queuePath");
            }

            QueuePath = queuePath;
        }

        public Task Push(string data)
        {
            return Task.Factory.StartNew(() =>
                {
                    var messageQueue = FindOrCreateQueue(QueuePath);
                    var message = new Message
                    {
                        Body = data,
                        Label = "AckMessage:" + Guid.NewGuid().ToString("N"),
                        Recoverable = true,
                    };
                    messageQueue.Send(message, MessageQueueTransactionType.Automatic);
                });
        }

        public Task<IQueueMessage> Peek()
        {
            return Task.Factory.StartNew(() =>
            {
                var queueMessage = default(IQueueMessage);
                var messageQueue = FindOrCreateQueue(QueuePath);

                try
                {
                    var message = messageQueue.Peek(new TimeSpan(0, 0, 1));
                    message.Formatter = message.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                    queueMessage = new QueueMessage
                    {
                        Id = message.Id,
                        Data = message.Body as string
                    };
                }
                catch { }

                return queueMessage;
            });
        }

        public Task Pop(IQueueMessage queueMessage)
        {
            return Task.Factory.StartNew(() =>
            {
                var messageQueue = FindOrCreateQueue(QueuePath);
                messageQueue.ReceiveById(queueMessage.Id, MessageQueueTransactionType.Automatic);
            });
        }

        private MessageQueue FindOrCreateQueue(string path)
        {
            if (!MessageQueue.Exists(QueuePath))
            {
                MessageQueue.Create(QueuePath);
            }

            var messageQueue = new MessageQueue(QueuePath);

            return messageQueue;
        }
    }
}

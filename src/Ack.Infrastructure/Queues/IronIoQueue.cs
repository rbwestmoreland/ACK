using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Ack.Infrastructure.Queues
{
    public class IronIoQueue : IQueue
    {
        private HttpClient HttpClient { get; set; }

        public IronIoQueue(string token,
            string projectId,
            string queueName,
            string queueUrl)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException("token");
            }

            if (string.IsNullOrWhiteSpace(projectId))
            {
                throw new ArgumentNullException("projectId");
            }

            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentNullException("queueName");
            }

            if (string.IsNullOrWhiteSpace(queueUrl))
            {
                throw new ArgumentNullException("queueUrl");
            }

            Uri queueUri;
            if (!Uri.TryCreate(queueUrl, UriKind.Absolute, out queueUri))
            {
                throw new ArgumentException("invalid", "queueUrl");
            }

            if (!Uri.TryCreate(queueUri, string.Format("/projects/{0}", projectId), out queueUri))
            {
                throw new ArgumentException("invalid", "projectId");
            }

            if (!Uri.TryCreate(queueUri, string.Format("/1/projects/{0}/queues/{1}/", projectId, queueName), out queueUri))
            {
                throw new ArgumentException("invalid", "queueName");
            }

            HttpClient = new HttpClient();
            HttpClient.BaseAddress = queueUri;
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);
        }

        public async Task Push(string data)
        {
            var requestObj = new
            {
                messages = new [] 
                {
                    new
                    {
                        body = data
                    }
                }
            };

            var requestJson = JsonConvert.SerializeObject(requestObj);
            var requestContent = new StringContent(requestJson);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "messages")
            {
                Content = requestContent
            };

            var response = await HttpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<IronIoPushResponse>(responseContent);
        }

        public async Task<IMessage> Peek()
        {
            var message = default(IMessage);

            var request = new HttpRequestMessage(HttpMethod.Get, "messages/peek");

            var response = await HttpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<IronIoPeekResponse>(responseContent);

            if (responseObj.Messages != null &&
                responseObj.Messages.Any())
            {
                var ironIoMessage = responseObj.Messages.First();
                message = new Message
                {
                    Id = ironIoMessage.Id,
                    Data = ironIoMessage.Body
                };
            }

            return message;
        }

        public async Task Pop(IMessage message)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, string.Format("messages/{0}", message.Id));

            var response = await HttpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<IronIoPopResponse>(responseContent);
        }

        private class IronIoPushResponse
        {
            public string[] Ids { get; set; }
            public string Msg { get; set; }
        }

        private class IronIoMessage
        {
            public string Id { get; set; }
            public string Body { get; set; }
            public int Timeout { get; set; }
        }

        private class IronIoPeekResponse
        {
            public IronIoMessage[] Messages { get; set; }
        }

        private class IronIoPopResponse
        {
            public string Msg { get; set; }
        }
    }
}

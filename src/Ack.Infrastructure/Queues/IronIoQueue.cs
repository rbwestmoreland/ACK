using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

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

            if (!Uri.TryCreate(queueUri, string.Format("/1/projects/{0}/queues/{1}/messages", projectId, queueName), out queueUri))
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
            var bytes = Encoding.UTF8.GetBytes(data);
            var body = Convert.ToBase64String(bytes);

            var requestObj = new
            {
                messages = new [] 
                {
                    new
                    {
                        body = body
                    }
                }
            };

            var requestJson = JsonConvert.SerializeObject(requestObj);
            var requestContent = new StringContent(requestJson);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, string.Empty)
            {
                Content = requestContent
            };

            var response = await HttpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseStatusCode = response.StatusCode;
            var responseObj = JsonConvert.DeserializeObject<PushResponse>(responseContent);
        }

        public async Task<IMessage> Peek()
        {
            throw new NotImplementedException();
        }

        public async Task Pop(IMessage message)
        {
            throw new NotImplementedException();
        }

        private class PushResponse
        {
            public string[] Ids { get; set; }
            public string Msg { get; set; }
        }
    }
}

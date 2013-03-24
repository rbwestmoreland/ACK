using Ack.Infrastructure.Queues;
using Ack.Web.Models.WebApi.Errors;
using Ack.Web.Models.WebApi.Event;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ack.Web.Controllers.WebApi
{
    public class EventsController : BaseApiController
    {
        private IQueue Queue { get; set; }

        public EventsController(IQueue queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException("queue");
            }

            Queue = queue;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(NewEvent model)
        {
            if (model == null)
            {
                var error = new Error { Message = "Request body required." };
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }

            if (string.IsNullOrWhiteSpace(model.Type))
            {
                var error = new Error { Message = "Type property required." };
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }

            if (string.IsNullOrWhiteSpace(model.Data))
            {
                var error = new Error { Message = "Data property required." };
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }

            if (IsLargerThan32Kb(model.Data))
            {
                var error = new Error { Message = "Data must be less than than 32KB." };
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }

            if (!model.Timestamp.HasValue)
            {
                var error = new Error { Message = "Timestamp property required." };
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }

            var json = JsonConvert.SerializeObject(model);
            await Queue.Push(json);

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        private static bool IsLargerThan32Kb(string value)
        {
            var maxBytes = 32767; //32KB
            return Encoding.UTF8.GetByteCount(value) > maxBytes;
        }
    }
}
using Ack.Web.Models.WebApi.Errors;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ack.Web.Controllers.WebApi
{
    public class ErrorsController : BaseApiController
    {
        [HttpDelete, HttpGet, HttpHead, HttpOptions, HttpPatch, HttpPost, HttpPut]
        public HttpResponseMessage CatchAll(string path)
        {
            var value = new Error 
            { 
                Message = string.Format("{0} was not found or does not accept the {1} method.", Request.RequestUri, Request.Method) 
            };
            return Request.CreateResponse(HttpStatusCode.NotFound, value);
        }
    }
}
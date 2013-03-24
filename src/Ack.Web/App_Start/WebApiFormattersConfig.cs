using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Ack.Web
{
    public static class WebApiFormattersConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings 
                { 
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            });
        }
    }
}
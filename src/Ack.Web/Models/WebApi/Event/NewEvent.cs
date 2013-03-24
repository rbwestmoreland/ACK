using System;

namespace Ack.Web.Models.WebApi.Event
{
    public class NewEvent
    {
        public string Type { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Data { get; set; }
    }
}
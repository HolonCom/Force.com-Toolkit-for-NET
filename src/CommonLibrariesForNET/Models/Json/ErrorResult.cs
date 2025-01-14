using System.Collections.Generic;
using Newtonsoft.Json;

namespace Salesforce.Common.Models.Json
{
    public class ErrorResult
    {
        [JsonProperty(PropertyName = "statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public List<string> Fields { get; set; }
    }
}

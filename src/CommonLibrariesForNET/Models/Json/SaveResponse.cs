using System.Collections.Generic;
using Newtonsoft.Json;

namespace Salesforce.Common.Models.Json
{
    public class SaveResponse
    {
        [JsonProperty(PropertyName = "hasErrors")]
        public bool HasErrors { get; set; }

        [JsonProperty(PropertyName = "results")]
        public List<SaveResult> Results { get; set; }
    }
}

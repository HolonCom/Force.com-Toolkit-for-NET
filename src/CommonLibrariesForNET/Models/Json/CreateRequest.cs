using System.Collections.Generic;
using Newtonsoft.Json;

namespace Salesforce.Common.Models.Json
{
    public class CreateRequest
    {
        [JsonProperty(PropertyName = "records")]
        public List<IAttributedObject> Records { get; set; }
    }
}

﻿using Newtonsoft.Json;

namespace Salesforce.Common.Models.Json
{
    public class SaveResult
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "referenceId")]
        public string ReferenceId { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public Error[] Errors { get; set; }
    }

}

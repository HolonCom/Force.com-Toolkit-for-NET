﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Salesforce.Common.Models.Json
{
    public class CreateRequest
    {
        [JsonProperty(PropertyName = "records")]
        public List<IAttributedObject> Records { get; set; }
    }
}

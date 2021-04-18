using iProcessHelper.JsonModels.JsonProcessModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonModels.JsonProcessModel
{
    public class ProcessModelMetadata
    {
        [JsonProperty("Schema")]
        public ProcessModelSchema Schema { get; set; }
    }
}

using iProcessHelper.JsonProcessModels.Short;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonProcessModel.Short
{
    public class ProcessModelMetadataShort
    {
        [JsonProperty("Schema")]
        public ProcessModelSchemaShort Schema { get; set; }
    }
}

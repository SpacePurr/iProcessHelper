using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.ProcessJsonModel
{
    public class ProcessModelMetadata
    {
        [JsonProperty("Schema")]
        public ProcessModelSchema Schema { get; set; }
    }
}

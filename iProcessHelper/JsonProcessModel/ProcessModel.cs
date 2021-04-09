using iProcessHelper.JsonProcessModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.ProcessJsonModel
{
    public class ProcessModel : IProcessModel
    {
        [JsonProperty("MetaData")]
        public ProcessModelMetadata Metadata { get; set; }
    }
}

using iProcessHelper.JsonModels.JsonProcessModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonModels.JsonProcessModels
{
    public class ProcessModel
    {
        [JsonProperty("MetaData")]
        public ProcessModelMetadata Metadata { get; set; }
    }
}

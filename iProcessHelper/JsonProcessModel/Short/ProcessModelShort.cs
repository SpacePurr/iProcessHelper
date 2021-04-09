using iProcessHelper.JsonProcessModel.Short;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonProcessModels.Short
{
    public class ProcessModelShort : IProcessModel
    {
        [JsonProperty("MetaData")]
        public ProcessModelMetadataShort Metadata { get; set; }
    }
}

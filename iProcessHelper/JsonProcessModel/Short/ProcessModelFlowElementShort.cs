using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonProcessModels.Short
{
    public class ProcessModelFlowElementShort
    {
        [JsonProperty("BL1")]
        public string TypeName { get; set; }
        [JsonProperty("UId")]
        public Guid UId { get; set; }
        [JsonProperty("A2")]
        public string Name { get; set; }
        [JsonProperty("CK4")]
        public Guid SchemaUId { get; set; }
    }
}

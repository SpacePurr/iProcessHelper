using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonProcessModels.Short
{
    public class ProcessModelSchemaShort
    {
        [JsonProperty("ManagerName")]
        public string ManagerName { get; set; }
        [JsonProperty("UId")]
        public Guid UId { get; set; }
        [JsonProperty("A2")]
        public string Name { get; set; }
        [JsonProperty("BK4")]
        public List<ProcessModelFlowElementShort> FlowElements { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonModels.JsonProcessModels
{
    public class ProcessModelSchema
    {
        [JsonProperty("ManagerName")]
        public string ManagerName { get; set; }
        [JsonProperty("UId")]
        public Guid UId { get; set; }
        [JsonProperty("A2")]
        public string Name { get; set; }
        [JsonProperty("BK4")]
        public List<ProcessModelFlowElement> FlowElements { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonModels.JsonProcessModels
{
    public class ProcessModelFlowElement
    {
        [JsonProperty("BL1")]
        public string TypeName { get; set; }
        [JsonProperty("UId")]
        public Guid UId { get; set; }
        [JsonProperty("A2")]
        public string Name { get; set; }
        [JsonProperty("CK4")]
        public Guid SchemaUId { get; set; }
        [JsonProperty("FC2")]
        public Guid EntitySchemanUId { get; set; }
        [JsonProperty("DZ5")]
        public int EntitySignal { get; set; }
        [JsonProperty("DZ12")]
        public string NewEntityChangedColumns { get; set; }
        [JsonProperty("DZ13")]
        public string EntityFilters { get; set; }
    }
}

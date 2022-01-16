using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonModels.JsonEntityModel
{
    public class EntityModelColumn
    {
        [JsonProperty("UId")]
        public Guid UId { get; set; }
        [JsonProperty("A2")]
        public string Name { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonModels.JsonEntityModel
{
    public class EntityModelSchema
    {
        [JsonProperty("D2")]
        public List<EntityModelColumn> Columns { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonModels.JsonEntityModel
{
    public class EntityModelMetadata
    {
        [JsonProperty("Schema")]
        public EntityModelSchema Schema { get; set; }
    }
}

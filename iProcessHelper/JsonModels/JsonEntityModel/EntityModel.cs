using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.JsonModels.JsonEntityModel
{
    public class EntityModel
    {
        [JsonProperty("MetaData")]
        public EntityModelMetadata Metadata { get; set; }
    }
}

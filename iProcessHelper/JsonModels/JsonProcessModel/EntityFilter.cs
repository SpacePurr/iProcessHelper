using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.JsonModels.JsonProcessModel
{
    class EntityFilter
    {
        [JsonProperty("className")]
        public string ClassName { get; set; }
        [JsonProperty("dataSourceFilters")]
        public string DataSourceFilters { get; set; }
    }
}

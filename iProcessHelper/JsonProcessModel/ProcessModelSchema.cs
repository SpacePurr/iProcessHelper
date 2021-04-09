using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iProcessHelper.ProcessJsonModel
{
    public class ProcessModelSchema
    {
        [JsonProperty("ManagerName")]
        public string ManagerName { get; set; }
    }
}

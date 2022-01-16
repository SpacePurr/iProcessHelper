using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace iProcessHelper.DBContexts.DBModels
{
    [Table("SysSettings")]
    public class SysSettings
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public List<SysSettingsValue> SysSettingsValues { get; set; }
    }
}

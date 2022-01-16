using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace iProcessHelper.DBContexts.DBModels
{
    [Table("SysSettingsValue")]
    public class SysSettingsValue
    {
        public Guid Id { get; set; }
        public string TextValue { get; set; }
        public SysSettings SysSettings { get; set; }
        public Guid SysSettingsId { get; set; }
    }
}

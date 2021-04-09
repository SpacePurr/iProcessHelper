using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.DBContexts.DBModels
{
    [Table("SysSettingsValue")]
    public class SysSettingsValue
    {
        public Guid Id { get; set; }
        public Guid SysSettingsId { get; set; }
        public string TextValue { get; set; }
    }
}

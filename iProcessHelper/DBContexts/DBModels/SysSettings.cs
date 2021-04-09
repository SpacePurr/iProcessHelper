using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.DBContexts.DBModels
{
    [Table("SysSettings")]
    public class SysSettings
    {
        public SysSettings()
        {
            SysSettingsValues = new HashSet<SysSettingsValue>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }

        public virtual ICollection<SysSettingsValue> SysSettingsValues { get; set; }
    }
}

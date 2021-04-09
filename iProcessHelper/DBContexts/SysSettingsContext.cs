using iProcessHelper.DBContexts.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.DBContexts
{
    class SysSettingsContext: DbContext
    {
        public SysSettingsContext() : base("DefaultConnection")
        {

        }
        public DbSet<SysSettings> SysSettings { get; set; }
        public virtual DbSet<SysSettingsValue> SysSettingsValues { get; set; }
    }
}

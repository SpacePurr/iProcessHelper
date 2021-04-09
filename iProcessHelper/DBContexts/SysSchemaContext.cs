using iProcessHelper.DBContexts.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.DBContexts
{
    class SysSchemaContext : DbContext
    {
        public SysSchemaContext() : base("DefaultConnection")
        {

        }
        public DbSet<SysSchema> SysSchemas { get; set; }
    }
}

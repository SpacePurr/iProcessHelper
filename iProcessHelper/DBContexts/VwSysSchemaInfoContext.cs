using iProcessHelper.DBContexts.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace iProcessHelper.DBContexts
{
    public class VwSysSchemaInfoContext : DbContext
    {
        public DbSet<VwSysSchemaInfo> VwSysSchemaInfos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
        }
    }
}

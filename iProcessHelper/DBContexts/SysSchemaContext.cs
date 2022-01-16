using iProcessHelper.DBContexts.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace iProcessHelper.DBContexts
{
    class SysSchemaContext : DbContext
    {
        public DbSet<SysSchema> SysSchemas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}

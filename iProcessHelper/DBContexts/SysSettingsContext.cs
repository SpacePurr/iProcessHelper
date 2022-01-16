using iProcessHelper.DBContexts.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace iProcessHelper.DBContexts
{
    public class SysSettingsContext : DbContext
    {
        public DbSet<SysSettings> SysSettings { get; set; }
        public DbSet<SysSettingsValue> SysSettingsValues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
        }
    }
}

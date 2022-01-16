using iProcessHelper.DBContexts.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace iProcessHelper.DBContexts
{
    class InfromationSchemaColumnsContext : DbContext
    {
        public IQueryable<InformationSchemaColumns> AllInformationSchemaColumns => InformationSchemaColumns.FromSqlRaw("SELECT * FROM INFORMATION_SCHEMA.COLUMNS");
        public DbSet<InformationSchemaColumns> InformationSchemaColumns { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
        }
    }
}

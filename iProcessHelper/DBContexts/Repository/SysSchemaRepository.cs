using iProcessHelper.DBContexts.DBModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.DBContexts.Repository
{
    class SysSchemaRepository
    {
        public static SysSchema CreateProcessSchema(SqlDataReader reader)
        {
            var sysSchema = new SysSchema
            {
                Id = Guid.Parse(reader["id"].ToString()),
                CreatedOn = DateTime.Parse(reader["CreatedOn"].ToString()),
                ManagerName = reader["ManagerName"].ToString(),
                UId = Guid.Parse(reader["UId"].ToString()),
                Name = reader["Name"].ToString(),
                Caption = reader["Caption"].ToString(),
                MetaData = (byte[])reader["MetaData"],
            };

            if (Guid.TryParse(reader["ParentId"].ToString(), out var parentId))
                sysSchema.ParentId = parentId;

            return sysSchema;
        }

        public static SysSchema CreateEntitySchema(SqlDataReader reader)
        {
            return new SysSchema
            {
                Id = Guid.Parse(reader["id"].ToString()),
                CreatedOn = DateTime.Parse(reader["CreatedOn"].ToString()),
                ManagerName = reader["ManagerName"].ToString(),
                UId = Guid.Parse(reader["UId"].ToString()),
                Name = reader["Name"].ToString(),
                Caption = reader["Caption"].ToString()
            };
        }
    }
}

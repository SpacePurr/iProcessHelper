using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.DBContexts.Repository;
using iProcessHelper.JsonModels.JsonProcessModels;
using iProcessHelper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Helpers
{
    class BackgroundLoadFromDBService
    {
        public static void Load(SqlConnection connection, BackgroundWorker worker, int count, string query, Action<SqlDataReader> action)
        {
            var command = new SqlCommand(query, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                double percent = 0;

                while (reader.Read())
                {
                    double step = 100.0 / count;
                    percent += step;
                    worker.ReportProgress((int)percent);

                    action(reader);
                }
            }
        }

        public static int GetCount(SqlConnection connection, string query)
        {
            var count = 0;

            SqlCommand command = new SqlCommand(query, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    count = int.Parse(reader["RowsCount"].ToString());
                }
            }

            return count;
        }

        public static void LoadParentProcesses(SqlConnection connection, BackgroundWorker worker, Action<ProcessTreeViewElement> updateAction)
        {
            var sqlExpression = "select COUNT(Id) as RowsCount from SysSchema where ManagerName='ProcessSchemaManager' and ParentId is null";
            var count = GetCount(connection, sqlExpression);

            sqlExpression = "select * from SysSchema where ManagerName='ProcessSchemaManager' and ParentId is null order by Caption";
            Load(connection, worker, count, sqlExpression, (reader) =>
            {
                var sysSchema = SysSchemaRepository.CreateProcessSchema(reader);

                var json = MetadataParser.Deserialize<ProcessModel>(sysSchema.MetaData);
                updateAction(new ProcessTreeViewElement
                {
                    SysSchema = sysSchema,
                    Json = json
                });
            });
        }

        public static void LoadChildProcesses(SqlConnection connection, BackgroundWorker worker, IEnumerable<ProcessTreeViewElement> processes, 
            Action<ProcessTreeViewElement, ProcessTreeViewElement> updateAction)
        {
            var sqlExpression = "select COUNT(Id) as RowsCount from SysSchema where ManagerName='ProcessSchemaManager' and ParentId is not null";
            var count = GetCount(connection, sqlExpression);

            sqlExpression = "select * from SysSchema where ManagerName='ProcessSchemaManager' and ParentId is not null order by Caption";
            Load(connection, worker, count, sqlExpression, (reader) =>
            {
                var sysSchema = SysSchemaRepository.CreateProcessSchema(reader);

                var parent = processes.FirstOrDefault(e1 => e1.SysSchema.Id == sysSchema.ParentId);

                if (parent != null)
                {
                    var json = MetadataParser.Deserialize<ProcessModel>(sysSchema.MetaData);
                    updateAction(parent, new ProcessTreeViewElement
                    {
                        SysSchema = sysSchema,
                        Json = json
                    });
                }
            });
        }

        public static void LoadEntities(SqlConnection connection, BackgroundWorker worker, Action<SysSchema> updateEvent)
        {
            var sqlExpression = "select COUNT(Id) as RowsCount from VwSysSchemaInfo where ManagerName='EntitySchemaManager'";
            var count = GetCount(connection, sqlExpression);

            sqlExpression = "SELECT * FROM VwSysSchemaInfo where ManagerName='EntitySchemaManager' ORDER BY Caption";
            Load(connection, worker, count, sqlExpression, (reader) =>
            {
                var sysSchema = SysSchemaRepository.CreateEntitySchema(reader);

                updateEvent(sysSchema);
            });
        }
    }
}

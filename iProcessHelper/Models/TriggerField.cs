using iProcessHelper.DBContexts;
using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.Helpers;
using iProcessHelper.JsonModels.JsonEntityModel;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iProcessHelper.Models
{
    public class TriggerField
    {
        private EntityColumn column;

        public SysSchema Entity { get; set; }

        public TriggerField(SysSchema sysSchema)
        {
            Entity = sysSchema;
        }

        public EntityColumn Column { get => column; 
            set 
            { 
                column = value;
                if(Column.UId == Guid.Empty)
                {
                    using (var connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Database1"].ConnectionString))
                    {
                        connection.Open();
                        /*string sqlExpression =
                            $"declare @detailObjectName nvarchar(255) = '{Entity.Name}';" +
                            $"declare @typeColumnName nvarchar(255) = '{Column.Name}';" +
                            "declare @typeColumnNameFull nvarchar(255) = @typeColumnName;" +
                            "declare @majorId int, @minorId int;" +

                            "select top 1 @majorId = p.major_id, @minorId = p.minor_id " +
                            "FROM sys.extended_properties p " +
                            "INNER JOIN sys.TABLES t ON p.major_id = t.object_id " +
                            "INNER JOIN sys.COLUMNS c ON c.object_id = t.object_id " +
                            "WHERE " +
                            "t.Name = @detailObjectName " +
                            "AND c.Name = @typeColumnNameFull " +
                            "AND p.name = 'TS.ColumnName' " +
                            "AND p.value = @typeColumnNameFull; " +

                            "declare @typeColumnUId uniqueidentifier = " +
                            "( " +
                                "select top 1 CONVERT(uniqueidentifier, p.value) " +
                            "from sys.extended_properties p " +
                            "where  " +
                            "p.major_id = @majorId " +
                            "and p.minor_id = @minorId " +
                            "and p.name = 'TS.EntitySchemaColumn.UId' " +
                            "); " +

                        "select @typeColumnUId as TypeColumnUId";

                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var uid = reader["TypeColumnUId"];
                                column.UId = Guid.Parse(reader["TypeColumnUId"].ToString());
                            }
                        }*/

                        string sqlExpression = $"select ordinal_position, column_name, col_description('\"{Entity.Name}\"'::REGCLASS, ordinal_position) as comment " +
                            $"from information_schema.columns where table_name = '{Entity.Name}' and column_name = '{Column.Name}'";

                        var command = new NpgsqlCommand(sqlExpression, connection);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var result = reader["comment"].ToString();
                                var commentData = result.Split(";");
                                var uidData = commentData.FirstOrDefault(x => x.StartsWith("TS.EntitySchemaColumn.UId"));
                                var uid = uidData.Split("=")[1];
                                column.UId = Guid.Parse(uid);
                            }
                        }

                        connection.Close();
                    }
                }
            } 
        }
        public ObservableCollection<EntityColumn> Columns { get; set; }
    }
}

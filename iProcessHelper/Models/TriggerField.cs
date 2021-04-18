using iProcessHelper.DBContexts.DBModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Creatio_7;User Id='sa';Password='0'"))
                    {
                        connection.Open();
                        string sqlExpression =
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
                                column.UId = Guid.Parse(reader["TypeColumnUId"].ToString());
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

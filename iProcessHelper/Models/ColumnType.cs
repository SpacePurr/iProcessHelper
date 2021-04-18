using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Models
{
    public class ColumnType
    {
        public static DataType Parse(string value)
        {
            switch (value)
            {
                case "nvarchar":
                    return DataType.STRING;
                case "int":
                    return DataType.INT;
                case "decimal":
                    return DataType.DECIMAL;
                case "bit":
                    return DataType.BOOL;
                case "date":
                case "datetime2":
                    return DataType.DATE_TIME;
                case "uniqueidentifier":
                    return DataType.GUID;
                case "varbinary":
                    return DataType.BYTE_ARRAY;
                default:
                    return DataType.STRING;
            }
        }
    }

    public enum DataType
    {
        STRING,
        INT,
        DECIMAL,
        BOOL,
        DATE_TIME,
        GUID,
        BYTE_ARRAY
    }
}

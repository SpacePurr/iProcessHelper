using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.DBContexts
{
    public class DBConnection
    {
        private static string connectionString;
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                    connectionString = ConfigurationManager.AppSettings["DefaultConnection"];

                return connectionString;
            }
        }
    }
}

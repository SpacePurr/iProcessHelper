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
    }
}

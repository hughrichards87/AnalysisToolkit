using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.BackEnd.Helpers
{
    public class SqlServerManagement
    {
        public static void CreateDatabase(string databaseName, SqlConnection sqlConnection)
        {
            if (sqlConnection != null)
            {
                string command = "Create Database " + databaseName;
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}

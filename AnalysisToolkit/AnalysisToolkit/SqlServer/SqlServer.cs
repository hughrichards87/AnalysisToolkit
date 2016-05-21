using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AnalysisToolkit.SqlServer
{
    public class SqlServer
    {
        private SqlConnection _sqlConnection;
        private List<SqlCommand> _sqlCommands;

        public SqlServer(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
            _sqlCommands = new List<SqlCommand>();            
        }

       

    }
}

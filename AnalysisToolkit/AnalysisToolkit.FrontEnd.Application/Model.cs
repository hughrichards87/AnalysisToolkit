using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.FrontEnd.Application
{
    class Model
    {
        System.Data.SqlClient.SqlConnection _sqlConnection;
        
        public Model()
        {
            _sqlConnection = new System.Data.SqlClient.SqlConnection();

        }
       

        public void CreateDatabase()
        {

            SqlServerManagement.CreateDatabase(_sqlConnection);
        }


    }
}

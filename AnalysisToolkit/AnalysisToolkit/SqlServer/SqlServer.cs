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

        public void ImportMultipleCSVsIntoNewTables(List<> sourceToDestinationMappings)
        {

        }

        public void ImportCSVIntoNewTable(string sourceFile, string destinationTable, string separator)
        {

            importCSVIntoNewTable(sourceFile, destinationTable, importDataColumnsFromFirstLine(sourceFile, separator), separator, 2);
        }


        public void ImportCSVIntoNewTable(string sourceFile, string destinationTable, string headerFile, string separator)
        {

            ImportCSVIntoNewTable(sourceFile, destinationTable, headerFile, separator, separator);
        }


        public void ImportCSVIntoNewTable(string sourceFile, string destinationTable, string headerFile, string SourceFileSeparator, string headerFileSeparator)
        {
            ImportCSVIntoNewTable(sourceFile, destinationTable, importDataColumns(headerFile, headerFileSeparator), SourceFileSeparator);
        }

        public void ImportCSVIntoNewTable(string sourceFile, string destinationTable, DataColumn[] columns, string separator)
        {
            importCSVIntoNewTable(sourceFile, destinationTable, columns, separator);
        }

        private void importCSVIntoNewTable(string sourceFile, string destinationTable, DataColumn[] columns, string separator)
        {
            importCSVIntoNewTable(sourceFile, destinationTable, columns, separator, 1);
        }

       



    }
}

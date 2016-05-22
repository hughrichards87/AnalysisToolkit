using AnalysisToolkit.FileSystem;
using AnalysisToolkit.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AnalysisToolkit.SqlServer
{
    class ImportCSVToTable 
    {

        #region variables and properties

        private string _sourceFile;
        private string _destinationTable;
        private string[] _separators;
        private int _firstLine = -1;
        private SqlConnection _sqlConnection;
        private CSVReader _csvReader;
        public SqlConnection SqlConnection
        {
            get
            {
                return _sqlConnection;
            }

            set
            {
                _sqlConnection = value;
            }
        }

        public string DestinationTable
        {
            get
            {
                return _destinationTable;
            }

            set
            {
                _destinationTable = value;
            }
        }

        public CSVReader CsvReader
        {
            get
            {
                return _csvReader;
            }

            set
            {
                _csvReader = value;
            }
        }

        #endregion

        #region constructors

        public ImportCSVToTable(CSVReader csvReader, SqlConnection sqlConnection, string destinationTable)
        {

            this.SqlConnection = sqlConnection;
            this.DestinationTable = destinationTable;
        }

       

        #endregion

        #region import

        public void Import()
        {
            importToNewTable(this.SourceFile, this.FirstLine, this.Separators, this.SqlConnection, this.DestinationTable);
        }
 
        /// <summary>
        /// 
        /// 
        /// Modified from https://gallery.technet.microsoft.com/scriptcenter/Import-Large-CSVs-into-SQL-216223d9
        /// </summary>
        private void importToNewTable(CSVToDataTable csvToDataTable, SqlConnection sqlConnection, string destinationTable)
        {
            System.Diagnostics.Stopwatch elapsed = new System.Diagnostics.Stopwatch();
            elapsed.Start();

            int zero = 0;
            int one = 1;
            int batchSize = 100000;
            Int64 rows = zero;
            int batchSizeIndex = zero;
            int numberOfColumns = zero;

            using (
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(
                                                            sqlConnection,
                                                            System.Data.SqlClient.SqlBulkCopyOptions.TableLock,
                                                            null
                                                          )
                    {
                        DestinationTableName = destinationTable,
                        BulkCopyTimeout = zero,
                        BatchSize = batchSize
                    }
                )
            {


                //copy table structure in case they have added something in there already
                using (DataTable datatable = this._dataTable.Copy())
                {
                    batchSizeIndex = datatable.Rows.Count;
                    numberOfColumns = datatable.Columns.Count;

                    foreach(string[] line in csvReader.ReadRecord())
                    {
                        datatable.Rows.Add(line);
                        batchSizeIndex += one;
                        if (batchSizeIndex == batchSize)
                        {
                            bulkcopy.WriteToServer(datatable);
                            datatable.Rows.Clear();
                            batchSizeIndex = zero;
                        }
                        rows += one;
                    }
                    bulkcopy.WriteToServer(datatable);
                    datatable.Rows.Clear();
                }
               

                string sqlQuery = @"SELECT * FROM " + destinationTable;
                using (SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(this._dataTable);
                }
               
            }

            elapsed.Stop();
        }

        #endregion
    }
}

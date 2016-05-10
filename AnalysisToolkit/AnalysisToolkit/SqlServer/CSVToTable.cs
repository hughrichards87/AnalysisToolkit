using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.SqlServer
{
    class CSVToTable
    {
        private string _source;
        private string _destination;
        private string _separator;
        private int _firstLine;
        private Columns _columns;
        private SqlConnection _sqlConnection;

        public string Source
        {
            get
            {
                return _source;
            }

            set
            {
                _source = value;
            }
        }

        public string Destination
        {
            get
            {
                return _destination;
            }

            set
            {
                _destination = value;
            }
        }

        public string Separator
        {
            get
            {
                return _separator;
            }

            set
            {
                _separator = value;
            }
        }


        public CSVToTable(string sourceFile, SqlConnection sqlConnection, string destinationTable, DataColumn[] columns, string separator)
        {

        }

 
        /// <summary>
        /// 
        /// 
        /// Modified from https://gallery.technet.microsoft.com/scriptcenter/Import-Large-CSVs-into-SQL-216223d9
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationTable"></param>
        /// <param name="columns"></param>
        /// <param name="separator"></param>
        private void importToNewTable(string sourceFile, SqlConnection sqlConnection, string destinationTable, DataColumn[] columns, string separator, int firstRow)
        {
            System.Diagnostics.Stopwatch elapsed = new System.Diagnostics.Stopwatch();
            elapsed.Start();
            int zero = 0;
            int one = 1;
            int batchSize = 100000;
            Int64 rows = zero;
            using (
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(
                                                            sqlConnection.ConnectionString,
                                                            System.Data.SqlClient.SqlBulkCopyOptions.TableLock
                                                          )
                    {
                        DestinationTableName = destinationTable,
                        BulkCopyTimeout = zero,
                        BatchSize = batchSize
                    }
                )
            {

                using (System.IO.StreamReader reader = new System.IO.StreamReader(sourceFile))
                {
                    using (DataTable datatable = new DataTable())
                    {

                        datatable.Columns.AddRange(columns);

                        int batchSizeIndex = zero;
                        string[] line;
                        string[] seperatorArray = new string[] { separator };
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine().Split(seperatorArray, StringSplitOptions.None);
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
                }
            }
            elapsed.Stop();
            Console.WriteLine((rows + " records imported in " + elapsed.Elapsed.TotalSeconds + " seconds."));
            Console.ReadLine();


        }

    }
}

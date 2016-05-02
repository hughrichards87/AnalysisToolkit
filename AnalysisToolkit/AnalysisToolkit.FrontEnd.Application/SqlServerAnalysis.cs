using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AnalysisToolkit.BackEnd.Analysis
{
    class SqlServerAnalysis
    {
        private SqlConnection _sqlConnection;
        private List<SqlCommand> _sqlCommands;


        public SqlServerAnalysis(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
            _sqlCommands = new List<SqlCommand>();
        }


        public void ImportCSV(string file, string destinationTable)
        {
            System.Diagnostics.Stopwatch elapsed = new System.Diagnostics.Stopwatch();
            elapsed.Start();
            int zero = 0;
            int one = 1;
            int batchSize = 100000;
            Int64 rows = zero; 

            using (
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(
                                                            _sqlConnection.ConnectionString,
                                                            System.Data.SqlClient.SqlBulkCopyOptions.TableLock
                                                          )
                                                        {
                                                            DestinationTableName = destinationTable,
                                                            BulkCopyTimeout = zero,
                                                            BatchSize = batchSize
                                                        }
                )
            {

                using (System.IO.StreamReader reader = new System.IO.StreamReader(file))
                {
                    using (DataTable datatable = new DataTable())
                    {

                        datatable.Columns.Add("Column1", typeof(System.String));

                        int batchSizeIndex = zero;
                        while (!reader.EndOfStream)
                        {
                            datatable.Rows.Add(reader.ReadLine());
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

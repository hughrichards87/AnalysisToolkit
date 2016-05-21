using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        private DataTable _dataTable;

        private string _columnsSource;
        private string[] _columnSeparators;
        private ColumnDefintions _columnDefintions;

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

        public int FirstLine
        {
            get
            {
                return _firstLine;
            }

            set
            {
                _firstLine = value;
            }
        }

        public string[] Separators
        {
            get
            {
                return _separators;
            }

            set
            {
                _separators = value;
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

        public string SourceFile
        {
            get
            {
                return _sourceFile;
            }

            set
            {
                _sourceFile = value;
            }
        }

        //public enum ColumnDefintionsLocation
        //{
        //    FirstLineOfSourceFile,
        //    SeparateHeaderFile
        //}


        public class ColumnDefintionsInDataFile : ColumnDefintions
        {
            private int _firstLine = 1;
            private int _numberOfRows = 1;
            private ColumnConcatenationRules _columnConcatenationRule;

            public enum ColumnConcatenationRules
            {
                ConcatenateAll,
                OnlyFirstNonBlankField,
                OnlyLastNonBlankField
            }

            public ColumnDefintionsInDataFile(int firstLine)
                :this(firstLine, 1)
            {
            }

            public ColumnDefintionsInDataFile(int firstLine, int numberOfRows)
                :  this(firstLine, 1, ColumnConcatenationRules.ConcatenateAll)
            {
            }

            public ColumnDefintionsInDataFile(int firstLine, int numberOfRows, ColumnConcatenationRules columnConcatenationRule)
            {
                this._firstLine = firstLine;
                this._numberOfRows = numberOfRows;
                this._columnConcatenationRule = columnConcatenationRule;
            }

        }

        public class ColumnsDefintionFile : ColumnDefintions
        {

            private string _sourceFile;
            private string _destinationTable;
            private string[] _separators;
            private int _firstLine = -1;

        }


        public abstract class ColumnDefintions
        {


        }

        #endregion

        #region constructors

        public ImportCSVToTable(string sourceFile, string seperator, ColumnDefintions columnDefintions, SqlConnection sqlConnection, string destinationTable)
            : this(sourceFile, new string[] { seperator }, columnDefintionsLocation, sqlConnection, destinationTable)
        { }


        public ImportCSVToTable(string sourceFile, string[] seperators, ColumnDefintions columnDefintions, SqlConnection sqlConnection, string destinationTable)
        {
            setColumns(columnDefintionsLocation, sourceFile, seperators);

            this.SourceFile = sourceFile;
            this.Separators = seperators;
            this.SqlConnection = sqlConnection;
            this.DestinationTable = destinationTable;
            this._dataTable = new DataTable(destinationTable);
        }

        internal void setColumns(ColumnDefintions columnDefintions, string headerFile, string[] seperators)
        {
            this._columnsSource = headerFile;
            this._separators = seperators;
            this._columnDefintionsLocation = columnDefintionsLocation;

            switch (columnDefintionsLocation)
            {
                case ColumnDefintionsLocation.FirstLineOfSourceFile:
                    importDataColumnsFromFirstLine(headerFile, seperators);
                    this.FirstLine = 2;
                    break;
                case ColumnDefintionsLocation.SeparateHeaderFile:
                    importDataColumns(headerFile, seperators);
                    this.FirstLine = 1;
                    break;
            }

            
        }


        private void importDataColumns(string headerFile, string[] separators)
        {
            string[] line;
            int zero = 0;
            string name;

            using (System.IO.StreamReader reader = new System.IO.StreamReader(headerFile))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().Split(separators, StringSplitOptions.None);
                    name = line[zero];
                    this._dataTable.Columns.Add(new DataColumn(name));
                }
            }

        }

        private void importDataColumnsFromFirstLine(string sourceFile, string[] separators)
        {
           
            string[] line;
            string name;

            using (System.IO.StreamReader reader = new System.IO.StreamReader(sourceFile))
            {
                line = reader.ReadLine().Split(separators, StringSplitOptions.None);
                int length = line.Length;
                for (int i = 0; i < length; i++)
                {
                    name = line[i];
                    this._dataTable.Columns.Add(new DataColumn(name));
                }

            }
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
        private void importToNewTable(string sourceFile, int firstRow, string[] seperators, SqlConnection sqlConnection, string destinationTable)
        {
            System.Diagnostics.Stopwatch elapsed = new System.Diagnostics.Stopwatch();
            elapsed.Start();

            int zero = 0;
            int one = 1;
            int batchSize = 100000;
            Int64 rows = zero;
            int batchSizeIndex = zero;
            string[] line;
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

                using (System.IO.StreamReader reader = new System.IO.StreamReader(sourceFile))
                {
                    //copy table structure in case they have added something in there already
                    using (DataTable datatable = this._dataTable.Copy())
                    {
                        batchSizeIndex = datatable.Rows.Count;
                        numberOfColumns = datatable.Columns.Count;

                        if (firstRow > one)
                        {
                            for (int i = zero; i < firstRow; i++)
                            {
                                reader.ReadLine();
                            }
                        }

                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine().Split(seperators, StringSplitOptions.None);
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

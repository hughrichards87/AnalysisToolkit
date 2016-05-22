using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.IO
{
    class CSVToDataTable
    { 
        private CSVReader _csvReader;
        private DataTable _dataTable;
        private ColumnDefintions _columnDefintions;

        public DataTable DataTable
        {
            get
            {
                return _dataTable;
            }

            set
            {
                _dataTable = value;
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

        public ColumnDefintions ColumnDefintions
        {
            get
            {
                return _columnDefintions;
            }

            set
            {
                _columnDefintions = value;
            }
        }

        public CSVToDataTable(CSVReader csvReader, ColumnDefintions columnDefitions)
        {
            this._csvReader = csvReader;
            this._columnDefintions = columnDefitions;
           
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

        private void LoadBatch(int numberOfRecords)
        {
            if (this._dataTable == null)
            {

            }
        }

    }
}

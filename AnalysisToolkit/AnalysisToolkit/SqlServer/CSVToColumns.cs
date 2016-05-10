using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.SqlServer
{
    class CSVToColumns : Columns
    {
        private string _source;
        private string _separator;
        private Format _format;

        public enum Format
        {
            firstLine,
            HeaderFile
        }

        public CSVToColumns(string source, string separator, Format format)
        {
            this._source = source;
            this._separator = separator;
            this._format = format;

            switch (format)
            {
                case Format.firstLine:
                    base.DataColumns = importDataColumnsFromFirstLine(source, separator);
                    break;
                case Format.HeaderFile:
                    this.DataColumns = importDataColumns(source, separator);
                    break;
            }

        }

        private DataColumn[] importDataColumns(string headerFile, string separator)
        {
            string[] seperatorArray = new string[] { separator };
            List<DataColumn> dataColumns = new List<DataColumn>();
            string[] line;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(headerFile))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().Split(seperatorArray, StringSplitOptions.None);
                    dataColumns.Add(new DataColumn(line[0]));
                }
            }
            return dataColumns.ToArray();

        }

        private DataColumn[] importDataColumnsFromFirstLine(string sourceFile, string separator)
        {
            string[] seperatorArray = new string[] { separator };
            List<DataColumn> dataColumns = new List<DataColumn>();
            string[] line;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(sourceFile))
            {
                line = reader.ReadLine().Split(seperatorArray, StringSplitOptions.None);
                int length = line.Length;
                for (int i = 0; i < length; i++)
                {
                    dataColumns.Add(new DataColumn(line[i]));
                }

            }
            return dataColumns.ToArray();

        }
    }
}

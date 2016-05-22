using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.IO
{
    
    public class ColumnDefintionsInCSVFile : ColumnDefintions
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

        public ColumnDefintionsInCSVFile(int firstLine)
            : this(firstLine, 1)
        {
        }

        public ColumnDefintionsInCSVFile(int firstLine, int numberOfRows)
            : this(firstLine, 1, ColumnConcatenationRules.ConcatenateAll)
        {
        }

        public ColumnDefintionsInCSVFile(int firstLine, int numberOfRows, ColumnConcatenationRules columnConcatenationRule)
        {
            this._firstLine = firstLine;
            this._numberOfRows = numberOfRows;
            this._columnConcatenationRule = columnConcatenationRule;

        }

        internal override void Load()
        {
            base.Load();
        }

    }

    public class ColumnsDefintionFile : ColumnDefintions
    {
        private CSVReader _csvReader;

        public ColumnsDefintionFile(CSVReader csvReader)
        {
        }

        internal override void Load()
        {
            base.Load();
        }
    }

    public abstract class ColumnDefintions
    {
        private List<DataColumn> _dataColumns;

        internal virtual void Load()
        {

        }
    }

}

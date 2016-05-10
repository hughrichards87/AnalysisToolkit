using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.SqlServer
{
    class Columns
    {
        private DataColumn[] _dataColumns;

        public DataColumn[] DataColumns
        {
            get
            {
                return _dataColumns;
            }

            set
            {
                _dataColumns = value;
            }
        }
    }
}

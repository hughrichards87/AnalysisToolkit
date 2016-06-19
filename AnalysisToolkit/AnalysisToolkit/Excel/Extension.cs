using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.Excel
{
    public static class Extension
    {
        public static Range GetOccupiedCellsSurrounding(this Range range)
        {
            System.Windows.Forms.SendKeys.Send("^(a)");
            Microsoft.Office.Interop.Excel.Application ExApp = range.Application as Microsoft.Office.Interop.Excel.Application;
            return ExApp.Selection as Microsoft.Office.Interop.Excel.Range;
        }

        public static bool ContainsData(this Range range)
        {
            long one = 1;
            if (range.CountLarge == one && range.Cells.Value2 != null)
            {
                return true;
            }
            else if (range.CountLarge == one && range.Cells.Value2 == null)
            {
                return false;
            }
            else
            {
                long length0 = range.Rows.CountLarge, length1 = range.Columns.CountLarge;
                long i = one, j = one;
                string reset = "", line;

                for (i = one; i <= length0; i++)
                {
                    line = reset;
                    for (j = one; j <= length1; j++)
                    {

                        if (((Range)range.Cells[i, j]).Value2 != null)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
    }
}

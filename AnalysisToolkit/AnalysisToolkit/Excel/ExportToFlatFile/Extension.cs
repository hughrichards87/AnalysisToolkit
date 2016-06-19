using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.Excel.ExportToFlatFile
{
    public static class Extension
    {
        public static void ExportToFlatFiles(this Workbook workbook, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {

        }

        public static void ExportToFlatFiles(this Workbook workbook, string fileNameFormat, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {

        }

        public static void ExportToFlatFile(this Worksheet worksheet, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            ExportToFlatFile(worksheet, "", separator, escapeCharacter, encoding, valueType);

        }

        public static void ExportToFlatFile(this Worksheet worksheet, string fileName, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            ExportToFlatFile(worksheet.UsedRange, fileName, separator, escapeCharacter, encoding, valueType);

        }

        public static void ExportToFlatFile(this Range range, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            ExportToFlatFile(range, "", separator, escapeCharacter, encoding, valueType);
        }

        public static void ExportToFlatFile(this Range range, string fileName, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            ExportToFlatFile(new Range[] { range }, "", separator, escapeCharacter, encoding, valueType);
        }

        public static void ExportToFlatFile(this Range[] ranges, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            ExportToFlatFile(ranges, "", separator, escapeCharacter, encoding, valueType);
        }

        public static void ExportToFlatFile(this Range[] ranges, string fileName, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            ExportToFlatFile exportToFlatFile = new ExportToFlatFile(ranges, fileName, separator, escapeCharacter, encoding, valueType);
            bool showProgressBar = false;
            exportToFlatFile.Export(showProgressBar);

        }
    }
}

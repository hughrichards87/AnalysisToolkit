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
            ExportToFlatFiles(workbook, "", "", separator, escapeCharacter, encoding, valueType);
        }

        public static void ExportToFlatFiles(this Workbook workbook, string fileNameFormat, string sheetNameReplacement, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            ExportToFlatFiles(workbook, fileNameFormat, sheetNameReplacement, separator, escapeCharacter, encoding, valueType, false);
        }

        public static void ExportToFlatFiles(this Workbook workbook, string fileNameFormat, string sheetNameReplacement, string separator, string escapeCharacter, Encoding encoding, ValueType valueType, bool showGUI)
        {

            ExportToFlatFile exportToFlatFile = new ExportToFlatFile(workbook, fileNameFormat, sheetNameReplacement, separator, escapeCharacter, encoding, valueType);
            exportToFlatFile.Export(showGUI);
        }

        #region worksheet
        public static void ExportToFlatFile(this Worksheet worksheet, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            ExportToFlatFile(worksheet, "", separator, escapeCharacter, encoding, valueType);

        }

        public static void ExportToFlatFile(this Worksheet worksheet, string fileName, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            ExportToFlatFile(worksheet, fileName, separator, escapeCharacter, encoding, valueType, false);
        }

        public static void ExportToFlatFile(this Worksheet worksheet, string fileName, string separator, string escapeCharacter, Encoding encoding, ValueType valueType, bool showGUI)
        {
            ExportToFlatFile(worksheet.UsedRange, fileName, separator, escapeCharacter, encoding, valueType, showGUI);
        }
        #endregion

        #region range
        public static void ExportToFlatFile(this Range range, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            ExportToFlatFile(range, "", separator, escapeCharacter, encoding, valueType);
        }

        public static void ExportToFlatFile(this Range range, string fileName, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            ExportToFlatFile(range, fileName, separator, escapeCharacter, encoding, valueType, false);
        }

        public static void ExportToFlatFile(this Range range, string fileName, string separator, string escapeCharacter, Encoding encoding, ValueType valueType, bool showGUI)
        {
            ExportToFlatFile exportToFlatFile = new ExportToFlatFile(new Range[] { range }, fileName, separator, escapeCharacter, encoding, valueType);
            exportToFlatFile.Export(showGUI);
        }
        #endregion
    }
}

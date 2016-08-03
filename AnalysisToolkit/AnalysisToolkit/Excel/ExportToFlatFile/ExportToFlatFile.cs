using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalysisToolkit.Excel.ExportToFlatFile.Splash;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace AnalysisToolkit.Excel.ExportToFlatFile
{
    internal class ExportToFlatFile
    {
        internal ExportSplashController splash = null;
        private Microsoft.Office.Interop.Excel.Application _application;
        private List<WriteToFlatFile> _rangeExportPackages;
        private long _overallRows;
        private string separator;
        private string escapeCharacter;
        private string endOfLine;
        private ValueType valueType;

        public ExportToFlatFile(Range[] ranges, string fileName, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
            :this(ranges, fileName, "", separator, escapeCharacter, encoding, valueType)
        {
        }

        public ExportToFlatFile(Range[] ranges, string fileNameFormat, string sheetNameReplacement, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            create(ranges, fileNameFormat, sheetNameReplacement, separator, escapeCharacter, encoding, valueType);

        }

        public ExportToFlatFile(Workbook workbook, string fileNameFormat, string sheetNameReplacement, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            List<Range> ranges = new List<Range>();
            foreach ( Worksheet sheet in workbook.Sheets)
            {
                ranges.Add(sheet.UsedRange);
            }
             
            create(ranges.ToArray(), fileNameFormat, sheetNameReplacement, separator, escapeCharacter, encoding, valueType);
        }

        private void create(Range[] ranges, string fileNameFormat, string sheetNameReplacement, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {

            this._application = ranges[0].Application;
            this._rangeExportPackages = packageRangesForExport(ranges, fileNameFormat, sheetNameReplacement, encoding);

            this.separator = separator;
            this.escapeCharacter = escapeCharacter;
            this.valueType = valueType;
            this.endOfLine = "\r\n";
        }

        private List<WriteToFlatFile> packageRangesForExport(Range[] ranges, string fileNameFormat, string sheetNameReplacement, Encoding encoding)
        {
            List<WriteToFlatFile> rangeExportPackages = new List<WriteToFlatFile>();
            var queryGrouped =
                            from range in ranges
                            group range by range.Worksheet
                            into sheets
                            orderby sheets.Key.Index
                            select sheets;

            List<Range> rangeList;
            int one = 1;
            int i = one;
            string empty = "";
            string sheetFileName;

            foreach (var sheet in queryGrouped)
            {
                rangeList = new List<Range>();
                foreach (var rangesPerSheet in sheet)
                {
                    rangeList.Add(rangesPerSheet);
                }

                if (sheetNameReplacement != empty &&  fileNameFormat.Contains(sheetNameReplacement))
                {
                    sheetFileName = fileNameFormat.Replace(sheetNameReplacement, sheet.Key.Name);
                }
                else
                {
                    sheetFileName = fileNameFormat + ( i > one ?  "_" + i.ToString() : empty);
                }

                rangeExportPackages.Add(new WriteToFlatFile(rangeList.ToArray(), sheetFileName, encoding));

                i++;
            }

            return rangeExportPackages;

        }

        internal void Export()
        {
            Export(false);
        }

        internal void Export(bool showGui)
        {
            if (_rangeExportPackages != null)
            {

                escapeCharacter = escapeCharacter.Trim();
                separator = separator.Trim();

                if (showGui)
                {
                    splash = new ExportSplashController(this._application);
                }

               _overallRows = _rangeExportPackages.Select(o => o.OverallRows).Aggregate((current, next) => current + next);


                foreach (WriteToFlatFile rangeExport in _rangeExportPackages)
                {
                    rangeExport.OnUpdate -= Writer_OnUpdate;
                    rangeExport.OnUpdate += Writer_OnUpdate;
                    rangeExport.Write(separator, escapeCharacter, endOfLine, valueType); //, showGui);
                    rangeExport.OnUpdate -= Writer_OnUpdate;

                }

                if (splash != null)
                {
                    splash.Finish();
                    splash = null;
                }
            }
        }
     

        private bool Writer_OnUpdate(long currentRow)
        {
            return splash.Update(currentRow, _overallRows);
        }


    }
}

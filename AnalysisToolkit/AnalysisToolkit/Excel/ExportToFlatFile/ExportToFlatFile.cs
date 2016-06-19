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
        private Range[] ranges;
        private string fileName;
        private string separator;
        private string escapeCharacter;
        private Encoding encoding;
        private ValueType valueType;

        public ExportToFlatFile(Range[] ranges, string fileName, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            this.ranges = ranges;
            this.fileName = fileName;
            this.separator = separator;
            this.escapeCharacter = escapeCharacter;
            this.encoding = encoding;
            this.valueType = valueType;
        }

        public void Export(string fileName, Range range, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {


        }

        public void SaveAs(Range range, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            string workbookName = range.Worksheet.Application.ActiveWorkbook.Name;
            if (workbookName.Contains("."))
            {
                workbookName = workbookName.Substring(0, workbookName.LastIndexOf('.'));
            }

            SaveAs(workbookName + '_' + range.Worksheet.Name + ".txt", range, separator, escapeCharacter, encoding, valueType);
        }

        public void SaveAs(Range[] ranges, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
        }

        public void SaveAs(string fileName, Range range, string separator, string escapeCharacter, Encoding encoding, ValueType valueType)
        {
            SaveFileDialog saveFileDialog = ShowSaveFileDialog(fileName);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName.ToString();
                Export(fileName, range, separator, escapeCharacter, encoding, valueType);
            }
        }

        internal void Export(bool showProgressBar)
        {
            escapeCharacter = escapeCharacter.Trim();
            separator = separator.Trim();

            WriteToFlatFile writer = new WriteToFlatFile(ranges, fileName, encoding, separator, escapeCharacter, "\r\n", valueType);

            if (showProgressBar)
            {
                splash = new ExportSplashController(ranges[0].Application);
                writer.OnUpdate -= Writer_OnUpdate;
                writer.OnUpdate += Writer_OnUpdate;
            }

            writer.Write();
            if (splash != null)
            {
                splash.Finish();
                splash = null;
            }
        }

        private bool Writer_OnUpdate(long currentRow, long overallRows)
        {
            return splash.Update(currentRow, overallRows);
        }

        public SaveFileDialog ShowSaveFileDialog(string fileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|Comma Separated Value Files (*.csv)|*.csv|All Files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.FileName = fileName;
            saveFileDialog.Title = "Export to Flat File";
            return saveFileDialog;
        }

    }
}

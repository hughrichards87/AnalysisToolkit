using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalysisToolkit.Excel.ExportToFlatFile
{
    class WriteToFlatFile
    {

        private Range[] _ranges;
        private string _fileName;
        private Encoding _encoding;
        private StreamWriter _streamWriter;
        internal delegate bool OnUpdateHandler(long currentRow);
        internal event OnUpdateHandler OnUpdate;

        public long OverallRows
        {
            get;
            set;
        }

        internal WriteToFlatFile(Range range, string fileName, Encoding encoding)
            : this(new Range[] { range }, fileName, encoding)
        {

        }

        internal WriteToFlatFile(Range[] ranges, string fileName, Encoding encoding)
        {
            this._ranges = ranges;
            this._fileName = fileName;
            this._encoding = encoding;
            this.OverallRows = ranges.Select(o => o.Rows.CountLarge).Aggregate((current, next) => current + next);

        }

        internal void Write(string separator, string escapeCharacter, string endOfLine, ValueType valueType)
        {
        //    Write(separator, escapeCharacter, endOfLine, valueType, false);
        //}

        //internal void Write(string separator, string escapeCharacter, string endOfLine, ValueType valueType, bool showGui)
        //{
            if (_ranges != null)
            {
                DialogResult result = DialogResult.Yes;
                FileMode fileMode = FileMode.Create;

                //if (showGui && File.Exists(_fileName))
                //{
                //    result = MessageBox.Show(
                //                            _fileName.Substring(_fileName.LastIndexOf('\\') + 1) + " already exists.\nDo you want to replace it?",
                //                            "Confirm Save As",
                //                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1
                //                            );
                //}

                if (result == DialogResult.Yes)
                {
                    StreamWriter streamWriter = new StreamWriter(new FileStream(_fileName, fileMode), _encoding);


                    if (_ranges.Length == 1 && _ranges[0].CountLarge == 1)
                    {
                        string val = valueType == ValueType.Value ? _ranges[0].Cells.Value : _ranges[0].Cells.Value2;
                        _streamWriter.WriteLine(escapeCharacter + val + escapeCharacter);
                        _streamWriter.Flush();
                    }
                    else
                    {
                        writeValues(_ranges, streamWriter, separator, escapeCharacter, endOfLine, valueType);
                    }

                    streamWriter.Close();
                }
            }
        }

        private void writeValues(Range[] ranges, StreamWriter streamWriter, string separator, string escapeCharacter, string endOfLine, ValueType valueType)
        {
            if (ranges != null)
            {
                long
                 zero = 0, one = 1,
                 i = one, j = one,
                 length0, length1, length2 = ranges.Length;

                long overalCurrentIndex = zero;
                bool cancelled = false;

                string
                    blank = "", format,
                    formatvalue = "{0}{1}{0}{2}", formatline = "{0}{1}{0}" + endOfLine, formatend = "{0}{1}{0}";

                long updateInterval = 100;
                Range range, r;
                object val = null;

                if (valueType == ValueType.Value)
                {
                    for (long k = zero; k < length2; k++)
                    {
                        range = ranges[k];
                        length0 = range.Rows.CountLarge;
                        length1 = range.Columns.CountLarge;


                        for (i = one; i <= length0; i++)
                        {
                            for (j = one; j <= length1; j++)
                            {
                                r = ((Range)range.Cells[i, j]);
                                val = ((object)r.Value) ?? blank;
                                if (j < length1)
                                {
                                    format = formatvalue;
                                }
                                else if (i < length0)
                                {
                                    format = formatline;
                                }
                                else
                                {
                                    format = formatend;
                                }
                                streamWriter.Write(string.Format(format, escapeCharacter, val.ToString(), separator));

                            }

                            overalCurrentIndex++;
                            if (overalCurrentIndex % updateInterval == zero)
                            {
                                streamWriter.Flush();
                                if (OnUpdate != null)
                                {
                                    if (OnUpdate(overalCurrentIndex))
                                    {// if true, cancelled
                                        cancelled = true;
                                        break;
                                    }
                                }
                            }

                        }
                        if (cancelled)
                        {
                            break;
                        }
                    }
                }
                else if (valueType == ValueType.Value2)
                {
                    for (long k = zero; k < length2; k++)
                    {
                        range = ranges[k];
                        length0 = range.Rows.CountLarge;
                        length1 = range.Columns.CountLarge;

                        for (i = one; i <= length0; i++)
                        {
                            for (j = one; j <= length1; j++)
                            {
                                r = ((Range)range.Cells[i, j]);
                                val = ((object)r.Value2) ?? blank;
                                if (j < length1)
                                {
                                    format = formatvalue;
                                }
                                else if (i < length0)
                                {
                                    format = formatline;
                                }
                                else
                                {
                                    format = formatend;
                                }
                                streamWriter.Write(string.Format(format, escapeCharacter, val.ToString(), separator));

                            }
                            overalCurrentIndex++;
                            if (overalCurrentIndex % updateInterval == zero)
                            {
                                streamWriter.Flush();
                                if (OnUpdate != null)
                                {
                                    if (OnUpdate(overalCurrentIndex))
                                    {// if true, cancelled
                                        cancelled = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (cancelled)
                        {
                            break;
                        }
                    }
                }

                streamWriter.Flush();
                if (OnUpdate != null)
                {
                    if (OnUpdate(overalCurrentIndex))
                    {// if true, cancelled
                        cancelled = true;
                    }
                }
            }
        }

    }
}

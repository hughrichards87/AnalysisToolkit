using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.Excel.ExportToFlatFile
{
    class WriteToFlatFile
    {

        internal delegate bool OnUpdateHandler(long currentRow, long overallRows);
        internal event OnUpdateHandler OnUpdate;

        private StreamWriter _streamWriter;
        private Range[] _ranges;
        private string _separator;
        private string _escapeCharacter;
        private string _endOfLine;
        private ValueType _valueType;

        internal WriteToFlatFile(Range range, string fileName, Encoding encoding, string separator, string escapeCharacter, string endOfLine, ValueType valueType)
            :this(new Range[] { range }, fileName, encoding, separator, escapeCharacter, endOfLine, valueType)
        {
        }

        internal WriteToFlatFile(Range[] ranges, string fileName, Encoding encoding, string separator, string escapeCharacter, string endOfLine, ValueType valueType)
        {
            _streamWriter = new StreamWriter(new FileStream(fileName, FileMode.Create), encoding);

            _ranges = ranges;
            _separator = separator;
            _escapeCharacter = escapeCharacter;
            _endOfLine = endOfLine;
            _valueType = valueType;
        }

        internal void Write()
        {
            if (_ranges != null)
            {
                if (_ranges.Length == 1 && _ranges[0].CountLarge == 1)
                {
                    string val = _valueType == ValueType.Value ? _ranges[0].Cells.Value : _ranges[0].Cells.Value2;
                    _streamWriter.WriteLine(_escapeCharacter + val + _escapeCharacter);
                    _streamWriter.Flush();
                }
                else
                {
                    writeValues(_ranges, _streamWriter, _separator, _escapeCharacter, _endOfLine, _valueType);
                }
            }
            _streamWriter.Close();
        }


        private void writeValues(Range[] ranges, StreamWriter streamWriter, string separator, string escapeCharacter, string endOfLine, ValueType valueType)
        {
            if (ranges != null)
            {
                long
                 zero = 0, one = 1,
                 i = one, j = one,
                 length0, length1, length2 = ranges.Length;

                long overalRows = ranges.Select(o => o.Rows.CountLarge).Aggregate((current, next) => current + next);
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
                                    if (OnUpdate(overalCurrentIndex, overalRows))
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
                                    if (OnUpdate(overalCurrentIndex, overalRows))
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
            }
        }
    }
}

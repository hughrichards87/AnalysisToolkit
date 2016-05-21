using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace AnalysisToolkit.FileSystem
{
    public class CSVReader : IDisposable
    {
        private StreamReader _streamReader;
        private DataTable _datatable;

        private string[] _recordSeparators;
        private string[] _fieldSeparators;
        private string[] _escapeCharacters;
        private string[] _textQualifiers;

        #region constructors
        public CSVReader(StreamReader streamReader, string[] recordSeparators)
            : this(streamReader,  recordSeparators, null, null, null)
        {

        }
        public CSVReader(StreamReader streamReader, string recordSeparator, string fieldSeparator, string escapeCharacter, string textQualifier)
            : this(streamReader, new string[] { recordSeparator}, new string[] { fieldSeparator }, new string[] { escapeCharacter }, new string[] { textQualifier })
        {
        }

        public CSVReader(StreamReader streamReader, string[] recordSeparators, string[] fieldSeparators, string[] escapeCharacters, string[] textQualifiers)
        {
            this._streamReader = streamReader;
            this.setDefaultSpecialCharacters(recordSeparators, fieldSeparators, escapeCharacters, textQualifiers);
        }

        private void setDefaultSpecialCharacters(string[] recordSeparators, string[] fieldSeparators, string[] escapeCharacters, string[] textQualifiers)
        {
            this._recordSeparators = recordSeparators;
            this._fieldSeparators = fieldSeparators;
            this._escapeCharacters = escapeCharacters;
            this._textQualifiers = textQualifiers;
        }
        #endregion


        public IEnumerable<string[]> ReadRecord()
        {
            string empty = "";
            string buffer = empty;
            string[] record;
            int zero = 0;
            int one = 1;
            int two = 2;
            int bufferSize = 2000;
            int recordsCount = zero;
            int i = zero;
            char[] charBuffer = new char[bufferSize];
            string[] recordsBufferSplit;
            
            while (!_streamReader.EndOfStream) //Peek() >= zero)
            {
                _streamReader.Read(charBuffer, zero, bufferSize);
                buffer += charBuffer;

                if (containsAtleastOneFullRecord(buffer))
                {
                    //found so split in to records
                    recordsBufferSplit = buffer.Split();
                    recordsCount =  recordsBufferSplit.Length - two;
                    for (i = zero; i < recordsCount; i++)
                    {
                        //split in to fields
                        record = recordsBufferSplit[i].Split();
                        yield return record;
                    }

                    //put the left over in the buffer and carry on
                    buffer = recordsBufferSplit[recordsCount + one];
                    continue;
                }
            }

            //last of the file... lets see if its a record by splitting in to fields
            yield return buffer.Split();
        }

        private bool containsAtleastOneFullRecord(string buffer)
        {
            bool found = false;

           
            return found;
        }

        public void Dispose()
        {
            this._streamReader.Dispose();
            this._streamReader = null;
            this._datatable.Dispose();
            this._datatable = null;

            this._recordSeparators = null;
            this._fieldSeparators = null;
            this._escapeCharacters = null;
            this._textQualifiers = null;
        }


        //public static List<int> AllIndexesOf(this string str, string value)
        //{
        //    if (String.IsNullOrEmpty(value))
        //        throw new ArgumentException("the string to find may not be empty", "value");
        //    List<int> indexes = new List<int>();
        //    for (int index = 0; ; index += value.Length)
        //    {
        //        index = str.IndexOf(value, index);
        //        if (index == -1)
        //            return indexes;
        //        indexes.Add(index);
        //    }
        //}

        //public static IEnumerable<int> AllIndexesOf(this string str, string value)
        //{
        //    if (String.IsNullOrEmpty(value))
        //        throw new ArgumentException("the string to find may not be empty", "value");
        //    for (int index = 0; ; index += value.Length)
        //    {
        //        index = str.IndexOf(value, index);
        //        if (index == -1)
        //            break;
        //        yield return index;
        //    }
        //}
    }
}

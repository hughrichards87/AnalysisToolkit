using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace AnalysisToolkit.IO
{
    public class CSVReader : IDisposable
    {
        private StreamReader _streamReader;

        private string[] _recordSeparators;
        private string[] _fieldSeparators;
        private string[] _escapeCharacters;
        private string[] _textQualifiers;

        private string[] _escapedRecordSeparators;
        private string[] _escapedFieldSeparators;
        private string[] _escapedTextQualifiers;
        private int _bufferSize = 2000;
        private int _firstLineOfData = -1;

        public int FirstLineOfData
        {
            get
            {
                return _firstLineOfData;
            }

            set
            {
                _firstLineOfData = value;
            }
        }

        public int BufferSize
        {
            get
            {
                return _bufferSize;
            }

            set
            {
                _bufferSize = value;
            }
        }

        #region constructors
        public CSVReader(StreamReader streamReader, string recordSeparator)
            : this(streamReader, new string[] { recordSeparator} )
        {

        }

        public CSVReader(StreamReader streamReader, string[] recordSeparators)
            : this(streamReader,recordSeparators, null, null, null)
        {

        }
        public CSVReader(StreamReader streamReader,  string recordSeparator, string fieldSeparator, string escapeCharacter, string textQualifier)
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

            this._escapedFieldSeparators = combineSpecialCharacters(escapeCharacters, fieldSeparators);
            this._escapedRecordSeparators = combineSpecialCharacters(escapeCharacters, recordSeparators);
            this._escapedTextQualifiers = combineSpecialCharacters(escapeCharacters, textQualifiers);

        }

        private string[] combineSpecialCharacters(string[] specialCharacters1, string[] specialCharacters2)
        {

            List<string> combined = new List<string>();
            int zero = 0;
            int i = zero, j = zero;
            int iLength = specialCharacters1 == null ? zero : specialCharacters1.Length;
            int jLength = specialCharacters2 == null ? zero : specialCharacters2.Length;

            for (; i < iLength; i++)
            {
                for (j = zero; j < jLength; j++)
                {
                    combined.Add(specialCharacters1[i] + specialCharacters2[j]);
                }
            }

            return combined.ToArray();
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
            
            int recordsCount = zero;
            int i = zero;
            char[] charBuffer = new char[_bufferSize];
            string[] recordsBufferSplit;
            
            while (!_streamReader.EndOfStream) //Peek() >= zero)
            {
                _streamReader.Read(charBuffer, zero, _bufferSize);
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
            string[] potentialRecordSplit =  buffer.Split(_fieldSeparators, StringSplitOptions.None);
            int zero = 0;
            int i = zero;
            int length = potentialRecordSplit.Length;
            for (; i < length; i++)
            {

            }

            bool found = false;
            return found;
        }

        public void Dispose()
        {
            this._streamReader.Dispose();
            this._streamReader = null;

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

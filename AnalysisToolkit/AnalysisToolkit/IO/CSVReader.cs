using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using AnalysisToolkit.Histograms;

namespace AnalysisToolkit.IO
{
    public class CSVReader : IDisposable
    {
        private StreamReader _streamReader;

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

        private SpecialStrings _specialStrings;

        private class SpecialStringInstructions 
        {
            int _offset;
            int _length;
            bool _consume;
            SpecialType _type;

            public SpecialStringInstructions(SpecialChar specialChar)
                :this(specialChar, 0)
            {
            }

            public SpecialStringInstructions(SpecialChar specialChar, int offset)
            {
                this._length = specialChar.Depth;
                this._type = specialChar.Type;
                this._offset = offset;
                this._consume = true;
            }
        }

        private class SpecialStrings
        {
            private List<SpecialChar> _tree;
            private List<SpecialChar> _currentBranches;

            internal SpecialStrings()
            {
                _tree = new List<SpecialChar>();
                _currentBranches = new List<SpecialChar>();
            }

            internal void AddRange(string[] values, SpecialType type)
            {
                int one = 1;
                if (values != null && values.Length > one)
                { 
                    int zero = 0;
                    int i = zero;
                    int iLength = values.Length;
                    char firstChar;
                    SpecialType firstCharType;
                    string v;
                    string theRest;
                    SpecialChar root;
                    int vLength;
                    SpecialType none = SpecialType.None;

                    for (; i < iLength; i++)
                    {
                        v = values[i];
                        vLength = v.Length;
                        if (vLength > zero)
                        {
                            firstChar = v[zero];
                            firstCharType = vLength > one ? none : type;
                            root = _tree.Find(x => x.Value == firstChar);
                            if (root == null)
                            {
                                root = new SpecialChar(firstChar, firstCharType, zero);
                                _tree.Add(root);
                            }

                            if (vLength > one)
                            {
                                theRest = v.Substring(one);
                                root.AddChildren(theRest, type);
                            }
                        }
                    }
                }
            }

            internal List<SpecialStringInstructions> CheckForSpecialType(char c)
            {
                int length = _currentBranches.Count;
                int one = 1;
                List<SpecialStringInstructions> instructions = new List<SpecialStringInstructions>();
                if (length > one)
                {
                    List<SpecialChar> newBranches = new List<SpecialChar>();
                    SpecialChar child;
                    SpecialChar branch;
                    for (int i = 0; i < length; i++)
                    {
                        branch = _currentBranches[i];
                        child = branch.CheckChildrenForSpecialType(c);
                        if (child != null)
                        {
                            newBranches.Add(child);
                        }
                        else
                        {
                            instructions.Add(new SpecialStringInstructions(branch, one));
                        }
                    }
                    _currentBranches = newBranches;
                }


                SpecialChar root = _tree.Find(x => x.Value == c);
                if (root != null)
                {
                    if (root.HasChildren)
                    {
                        _currentBranches.Add(root);
                    }
                    else
                    {
                        instructions.Add(new SpecialStringInstructions(root));
                    }

                }
                return instructions;
            }
        }

        internal enum SpecialType
        {
            RecordSeparator,
            FieldSeparator,
            EscapeCharacter,
            TextQualifier,
            None
        }

        private class SpecialChar
        {
            private char _value;
            private SpecialType _type;
            private int _depth;
            private List<SpecialChar> _children;
            private SpecialChar _parent;

            internal char Value
            {
                get
                {
                    return _value;
                }

                set
                {
                    _value = value;
                }
            }

            internal SpecialType Type
            {
                get
                {
                    return _type;
                }

                set
                {
                    _type = value;
                }
            }

            internal int Depth
            {
                get
                {
                    return _depth;
                }

                set
                {
                    _depth = value;
                }
            }

            internal List<SpecialChar> Children
            {
                get
                {
                    return _children;
                }

                set
                {
                    _children = value;
                }
            }

            internal SpecialChar Parent
            {
                get
                {
                    return _parent;
                }

                set
                {
                    _parent = value;
                }
            }

            public bool HasChildren {
                get
                {
                    return _children != null; 
                }
            }

            internal SpecialChar(char value, SpecialType type, int depth)
                : this(null, value, type)
            {
                this._depth = depth;
            }

            internal SpecialChar(SpecialChar parent, char value, SpecialType type)
            {
                this._value = value;
                this._type = type;
                this._parent = parent;
                if (parent != null)
                {
                    this._depth = parent.Depth + 1;
                }

            }

            internal void AddChildren(string value, SpecialType type)
            {
                int length = value.Length;
                int one = 1;
                if (length >= one)
                {
                    if (_children == null)
                    {
                        _children = new List<SpecialChar>();
                    }
                    char firstChar = value[0];
                    SpecialType firstCharType = value.Length > one ? SpecialType.None : type;
                    SpecialChar child = _children.Find(x => x.Value == firstChar);
                    if (child == null)
                    {
                        child = new SpecialChar(this, firstChar, firstCharType);
                        _children.Add(child);
                    }

                    if (length > one)
                    {
                        child.AddChildren(value.Substring(one), type);
                    }
                }
            }

            internal SpecialChar CheckChildrenForSpecialType(char c)
            {
                return _children.Find(x => x.Value == c);
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
            _specialStrings = new SpecialStrings();

            this._specialStrings.AddRange(recordSeparators, SpecialType.RecordSeparator);
            this._specialStrings.AddRange(fieldSeparators, SpecialType.FieldSeparator);
            this._specialStrings.AddRange(escapeCharacters, SpecialType.EscapeCharacter);
            this._specialStrings.AddRange(textQualifiers, SpecialType.TextQualifier);

        }

        
        #endregion


        public IEnumerable<string[]> ReadRecord()
        {
            List<SpecialStringInstructions> instructions;
            char c;
            while (!_streamReader.EndOfStream) //Peek() >= zero)
            {
                c = (char)_streamReader.Read();
                instructions = _specialStrings.CheckForSpecialType(c);

            }

            //last of the file... lets see if its a record by splitting in to fields
            yield return null;
        }

         public void Dispose()
        {
            this._streamReader.Dispose();
            this._streamReader = null;


        }
    }
}

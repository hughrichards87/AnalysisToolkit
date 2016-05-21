using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.SqlServer
{
    class ImportFilesToTables : IList<ImportCSVToTable>
    {
        private List<ImportCSVToTable> _tablesToImport;
        private SqlConnection _sqlConnection;

        #region IList

        public ImportCSVToTable this[int index]
        {
            get
            {
                return this._tablesToImport[index];
            }

            set
            {
                this._tablesToImport[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return this._tablesToImport.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(ImportCSVToTable item)
        {
            this._tablesToImport.Add(item);
        }


        public void Clear()
        {
            this._tablesToImport.Clear();
        }

        public bool Contains(ImportCSVToTable item)
        {
            return this._tablesToImport.Contains(item);
        }

        public void CopyTo(ImportCSVToTable[] array, int arrayIndex)
        {
            this._tablesToImport.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ImportCSVToTable> GetEnumerator()
        {
            return this._tablesToImport.GetEnumerator();
        }

        public int IndexOf(ImportCSVToTable item)
        {
            return this._tablesToImport.IndexOf(item);
        }

        public void Insert(int index, ImportCSVToTable item)
        {
            this._tablesToImport.Insert(index, item);
        }

        public bool Remove(ImportCSVToTable item)
        {
            return this._tablesToImport.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this._tablesToImport.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._tablesToImport.GetEnumerator();
        }

        #endregion

        public ImportFilesToTables(ImportCSVToTable fileToTableMapping)
        {
            Add(fileToTableMapping);
        }

        public ImportFilesToTables(List<ImportCSVToTable> fileToTableMappings)
        {
            this.AddRange(fileToTableMappings);
        }

        private void AddRange(List<ImportCSVToTable> collection)
        {
            this._tablesToImport.AddRange(collection);
        }

        public ImportFilesToTables(List<ImportCSVToTable> fileToTableMappings, string separator, ImportCSVToTable.ColumnDefintionsLocation columnDefintionsLocation, SqlConnection sqlConnection)
        {
            string[] separators = new string[] { separator };
            this._sqlConnection = sqlConnection;
            int count = fileToTableMappings.Count;
            this._tablesToImport = new List<ImportCSVToTable>();
            ImportCSVToTable fileToTable = null;

            for (int i = 0; i < count; i++)
            {
                fileToTable = fileToTableMappings[i];
                this._tablesToImport.Add(new ImportCSVToTable(fileToTable.SourceFile, separators, columnDefintionsLocation, sqlConnection, fileToTable.DestinationTable));
            }                     
        }

        public void Import()
        {
            if (_tablesToImport != null)
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    this._tablesToImport[i].Import();
                }
            }
        }
    }
}

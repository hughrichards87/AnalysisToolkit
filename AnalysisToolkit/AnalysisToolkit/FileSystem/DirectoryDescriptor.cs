using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.FileSystem
{
    public class DirectoryDescriptor
    {

        private string _directoryPath = "";
        private bool _includeSubFolders = true;


        public DirectoryDescriptor(string directoryPath, bool includeSubFolders)
        {
            this._directoryPath = directoryPath;
            this._includeSubFolders = includeSubFolders;
        }
    }
}

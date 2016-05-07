using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.FileSystem
{
    public class DirectoryWatcher
    {
        private WatcherEx _fileWatcher = null;
        private string _directoryPath = "";
        private bool _includeSubFolders = true;


        public DirectoryWatcher(string directoryPath, bool includeSubFolders)
        {
            this._directoryPath = directoryPath;
            this._includeSubFolders = includeSubFolders;
        }

        #region Helper Methods
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Toggles the Watcher event handlers on/off
        /// </summary>
        /// <param name="add"></param>
        private void ManageEventHandlers(bool add)
        {
            // For the purposes of this demo, I funneled all of the change events into 
            // one handler to keep the code base smaller. You could certainly have an 
            // individual handler for each type of change event if you desire.
            if (_fileWatcher != null)
            {
                if (add)
                {
                    _fileWatcher.EventChangedAttribute += new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedCreationTime += new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedDirectoryName += new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedFileName += new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedLastAccess += new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedLastWrite += new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedSecurity += new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedSize += new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventCreated += new WatcherExEventHandler(fileWatcher_EventCreated);
                    _fileWatcher.EventDeleted += new WatcherExEventHandler(fileWatcher_EventDeleted);
                    _fileWatcher.EventDisposed += new WatcherExEventHandler(fileWatcher_EventDisposed);
                    _fileWatcher.EventError += new WatcherExEventHandler(fileWatcher_EventError);
                    _fileWatcher.EventRenamed += new WatcherExEventHandler(fileWatcher_EventRenamed);
                    _fileWatcher.EventPathAvailability += new WatcherExEventHandler(fileWatcher_EventPathAvailability);
                }
                else
                {
                    _fileWatcher.EventChangedAttribute -= new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedCreationTime -= new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedDirectoryName -= new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedFileName -= new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedLastAccess -= new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedLastWrite -= new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedSecurity -= new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventChangedSize -= new WatcherExEventHandler(fileWatcher_EventChanged);
                    _fileWatcher.EventCreated -= new WatcherExEventHandler(fileWatcher_EventCreated);
                    _fileWatcher.EventDeleted -= new WatcherExEventHandler(fileWatcher_EventDeleted);
                    _fileWatcher.EventDisposed -= new WatcherExEventHandler(fileWatcher_EventDisposed);
                    _fileWatcher.EventError -= new WatcherExEventHandler(fileWatcher_EventError);
                    _fileWatcher.EventRenamed -= new WatcherExEventHandler(fileWatcher_EventRenamed);
                    _fileWatcher.EventPathAvailability += new WatcherExEventHandler(fileWatcher_EventPathAvailability);
                }
            }
        }

        private void fileWatcher_EventPathAvailability(object sender, WatcherExEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void fileWatcher_EventError(object sender, WatcherExEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void fileWatcher_EventRenamed(object sender, WatcherExEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void fileWatcher_EventDeleted(object sender, WatcherExEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void fileWatcher_EventDisposed(object sender, WatcherExEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void fileWatcher_EventCreated(object sender, WatcherExEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void fileWatcher_EventChanged(object sender, WatcherExEventArgs e)
        {
            throw new NotImplementedException();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the Watcher object. In the interest of providing a complete 
        /// demonstration, all change events are monitored. This will unlikely be a 
        /// real-world requirement in most cases.
        /// </summary>
        private WatcherEx InitWatcher(string directoryPath, bool includeSubFolders)
        {
            WatcherEx fileWatcher = null;
            if (Directory.Exists(directoryPath) ) //|| File.Exists(this.textBoxFolderToWatch.Text))
            {
                WatcherInfo info = new WatcherInfo();
                info.ChangesFilters = NotifyFilters.Attributes |
                                      NotifyFilters.CreationTime |
                                      NotifyFilters.DirectoryName |
                                      NotifyFilters.FileName |
                                      NotifyFilters.LastAccess |
                                      NotifyFilters.LastWrite |
                                      NotifyFilters.Security |
                                      NotifyFilters.Size;

                info.IncludeSubFolders = includeSubFolders;
                info.WatchesFilters = WatcherChangeTypes.All;
                info.WatchForDisposed = true;
                info.WatchForError = false;
                info.WatchPath = directoryPath;
                info.BufferKBytes = 8;
                info.MonitorPathInterval = 250;
                fileWatcher = new WatcherEx(info);
                ManageEventHandlers(true);
               
            }
            else
            {
                //TO DO: throw event
                //MessageBox.Show("The folder (or file) specified does not exist.\nPlease specify a valid folder or filename.");
            }
            return fileWatcher;
        }
        #endregion Helper Methods
    }
}

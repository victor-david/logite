using Restless.Logite.Core;
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using Restless.Logite.Resources;
using Restless.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Provides the logic that is used to display the log files.
    /// </summary>
    public class FileViewModel : DataGridViewModel<ImportFileTable>
    {
        #region Private
        #endregion

        /************************************************************************/

        #region Properties
        public ObservableCollection<LogFile> LogFiles
        {
            get;
        }

        public DataGridColumnCollection LogFileColumns
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="FileViewModel"/> class.
        /// </summary>
        public FileViewModel() : base()
        {
            DisplayName = Strings.MenuItemImport;

            Columns.Create("Id", ImportFileTable.Defs.Columns.Id).MakeFixedWidth(FixedWidth.W042);
            Columns.Create("Name", ImportFileTable.Defs.Columns.FileName);
            Columns.Create("Lines", ImportFileTable.Defs.Columns.LineCount).MakeFixedWidth(FixedWidth.W052);

            LogFiles = new ObservableCollection<LogFile>();
            LogFileColumns = new DataGridColumnCollection();
            LogFileColumns.Create("File", nameof(LogFile.DisplayName));
            LogFileColumns.Create("Domain", nameof(LogFile.Domain)).MakeFixedWidth(FixedWidth.W096);
            LogFileColumns.Create("Status", nameof(LogFile.Status)).MakeFixedWidth(FixedWidth.W136);

            Commands.Add("Import", RunImportCommand);
        }
        #endregion

        /************************************************************************/

        #region Protected Methods
        protected override void OnActivated()
        {
            UpdatePending();
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void UpdatePending()
        {
            LogFiles.Clear();
            foreach (string file in System.IO.Directory.EnumerateFiles(Config.LogFileDirectory))
            {
                LogFile logFile = new(file);
                SetLogFileDomain(logFile);
                SetLogFileStatus(logFile);
                LogFiles.Add(logFile);
            }
        }

        private void SetLogFileDomain(LogFile logFile)
        {
            foreach(DomainRow domain in DatabaseController.Instance.GetTable<DomainTable>().EnumerateAll())
            {
                if (logFile.DisplayName.StartsWith(domain.Preface, StringComparison.InvariantCulture))
                {
                    logFile.DomainId = domain.Id;
                    logFile.Domain = domain.DisplayName;
                    return;
                }
            }
        }
        private void SetLogFileStatus(LogFile logFile)
        {
            if (logFile.DomainId == LogFile.UninitializedDomaindId)
            {
                logFile.Status = LogFile.StatusIneligible;
                return;
            }

            ImportFileRow logFileRow = DatabaseController.Instance.GetTable<ImportFileTable>().GetSingleRecord(logFile.DomainId, logFile.DisplayName);
            logFile.Status = logFileRow == null ? LogFile.StatusReady : LogFile.StatusImported;
        }

        private void RunImportCommand(object parm)
        {
            try
            {
                LogEntryProcessor.Init();
                List<LogFile> processed = new List<LogFile>();

                foreach (LogFile logFile in LogFiles.Where(lf => lf.Status == LogFile.StatusReady))
                {
                    string[] lines = System.IO.File.ReadAllLines(logFile.Path);
                    foreach (string line in lines)
                    {
                        LogEntry entry = LineParser.ParseLine(line);
                        entry.DomainId = logFile.DomainId;
                        LogEntryProcessor.Process(entry);
                    }

                    processed.Add(logFile);
                }
                
                DatabaseController.Instance.Save();

                foreach (LogFile logFile in processed)
                {
                    LogFiles.Remove(logFile);
                }
            }
            catch (Exception ex)
            {
                MessageWindow.ShowError(ex.Message);
            }
        }
        #endregion
    }
}
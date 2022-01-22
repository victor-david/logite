using Restless.Logite.Core;
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using Restless.Logite.Network;
using Restless.Logite.Resources;
using Restless.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Provides the logic that is used to display and import the raw log files.
    /// </summary>
    public class ImportViewModel : DataGridViewModel<ImportFileTable>
    {
        #region Private
        private bool operationInProgress;
        #endregion

        /************************************************************************/

        #region Properties
        public ObservableCollection<ImportFile> ImportFiles
        {
            get;
        }

        public DataGridColumnCollection ImportFileColumns
        {
            get;
        }

        /// <summary>
        /// Gets a value that indicates if ab async operation is in progress
        /// </summary>
        public bool OperationInProgress
        {
            get => operationInProgress;
            private set => SetProperty(ref operationInProgress, value);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportViewModel"/> class.
        /// </summary>
        public ImportViewModel() : base()
        {
            DisplayName = Strings.MenuItemImport;

            Columns.Create("Id", ImportFileTable.Defs.Columns.Id).MakeFixedWidth(FixedWidth.W042);
            Columns.Create("Name", ImportFileTable.Defs.Columns.FileName);
            Columns.Create("Lines", ImportFileTable.Defs.Columns.LineCount).MakeFixedWidth(FixedWidth.W052);

            ImportFiles = new ObservableCollection<ImportFile>();
            ImportFileColumns = new DataGridColumnCollection();
            ImportFileColumns.Create("File", nameof(ImportFile.FileName));
            ImportFileColumns.Create("Domain", nameof(ImportFile.Domain)).MakeFixedWidth(FixedWidth.W096);
            ImportFileColumns.Create("Status", nameof(ImportFile.Status)).MakeFixedWidth(FixedWidth.W136);

            Commands.Add("Download", RunDownloadCommand, (p) => !OperationInProgress);
            Commands.Add("Import", RunImportCommand, (p) => !OperationInProgress);
        }
        #endregion

        /************************************************************************/

        #region Protected Methods
        protected override void OnActivated()
        {
            UpdatePending();
        }

        protected override int OnDataRowCompare(DataRow item1, DataRow item2)
        {
            return DataRowCompareLong(item2, item1, ImportFileTable.Defs.Columns.Id);
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void UpdatePending()
        {
            ImportFiles.Clear();
            foreach (string file in System.IO.Directory.EnumerateFiles(Config.LocalLogDirectory))
            {
                ImportFile importFile = new(file);
                importFile.Domain = GetRelatedDomain(importFile.FileName);

                SetImportFileStatus(importFile);
                if (importFile.Status != ImportFile.StatusImported)
                {
                    ImportFiles.Add(importFile);
                }
            }
        }

        private DomainRow GetRelatedDomain(string fileName)
        {
            foreach(DomainRow domain in DatabaseController.Instance.GetTable<DomainTable>().EnumerateAll())
            {
                if (fileName.StartsWith(domain.Preface, StringComparison.InvariantCulture))
                {
                    return domain;
                }
            }
            return null;
        }

        private void SetImportFileStatus(ImportFile importFile)
        {
            if (importFile.Domain == null)
            {
                importFile.Status = ImportFile.StatusIneligible;
                return;
            }

            ImportFileRow importFileRow = DatabaseController.Instance.GetTable<ImportFileTable>().GetSingleRecord(importFile.Domain.Id, importFile.FileName);
            importFile.Status = importFileRow == null ? ImportFile.StatusReady : ImportFile.StatusImported;
        }

        private async void RunDownloadCommand(object parm)
        {
            try
            {
                OperationInProgress = true;
                FtpConnectionConfig config = new FtpConnectionConfig(Config.FtpHost, Config.FtpUserName, Config.FtpKeyFile, Config.RemoteLogDirectory, Config.LocalLogDirectory);
                FtpClient ftp = new FtpClient(config);
                await ftp.DownloadLogFilesAsync((file) => IncludeDownloadFile(file));
                UpdatePending();
            }
            catch (Exception ex)
            {
                MessageWindow.ShowError(ex.Message);
            }
            finally
            {
                OperationInProgress = false;
            }
        }

        private bool IncludeDownloadFile(FtpFile file)
        {
            string localFile = Path.Combine(Config.LocalLogDirectory, file.Name);
            if (File.Exists(localFile) && !Config.OverwriteLogFiles)
            {
                return false;
            }

            DomainRow domain = GetRelatedDomain(file.Name);
            /* file is eligible if:
             * it has a related domain and 
             * the file name matches the regular expression (if one exists) */
            return domain != null && (string.IsNullOrEmpty(Config.LogFileRegex) || Regex.IsMatch(file.Name, Config.LogFileRegex));
        }

        private void RunImportCommand(object parm)
        {
            try
            {
                ImportFileTable importTable = DatabaseController.Instance.GetTable<ImportFileTable>();

                ApplicationTableBase[] tables = 
                {
                    importTable,
                    DatabaseController.Instance.GetTable<LogEntryTable>(),
                    DatabaseController.Instance.GetTable<IpAddressTable>(),
                    DatabaseController.Instance.GetTable<RefererTable>(),
                    DatabaseController.Instance.GetTable<RequestTable>(),
                    DatabaseController.Instance.GetTable<StatusTable>(),
                    DatabaseController.Instance.GetTable<UserAgentTable>(),
                };

                DatabaseController.Instance.Transaction.ExecuteTransaction((transaction) =>
                {
                    LogEntryProcessor.Init();
                    List<ImportFile> processed = new List<ImportFile>();

                    foreach (ImportFile logFile in ImportFiles.Where(lf => lf.Status == ImportFile.StatusReady))
                    {
                        string[] lines = System.IO.File.ReadAllLines(logFile.Path);
                        ImportFileRow import = importTable.Create(logFile.FileName, logFile.Domain.Id, lines.LongLength);
                        long lineNumber = 0;
                        foreach (string line in lines)
                        {
                            LogEntry entry = LineParser.ParseLine(line, logFile.Domain.Id, import.Id, lineNumber);
                            LogEntryProcessor.Process(entry);
                            lineNumber++;
                        }

                        processed.Add(logFile);
                    }

                    foreach (ImportFile logFile in processed)
                    {
                        ImportFiles.Remove(logFile);
                    }

                    foreach (ApplicationTableBase table in tables)
                    {
                        table.Save(transaction);
                    }
                }, tables);

                DatabaseController.Instance.GetTable<DomainTable>().UpdateLogEntryCount();

                Refresh();
            }
            catch (Exception ex)
            {
                MessageWindow.ShowError(ex.Message);
            }
        }
        #endregion
    }
}
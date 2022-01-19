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
        public ObservableCollection<ImportFile> ImportFiles
        {
            get;
        }

        public DataGridColumnCollection ImportFileColumns
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

            ImportFiles = new ObservableCollection<ImportFile>();
            ImportFileColumns = new DataGridColumnCollection();
            ImportFileColumns.Create("File", nameof(ImportFile.DisplayName));
            ImportFileColumns.Create("Domain", nameof(ImportFile.Domain)).MakeFixedWidth(FixedWidth.W096);
            ImportFileColumns.Create("Status", nameof(ImportFile.Status)).MakeFixedWidth(FixedWidth.W136);

            Commands.Add("Import", RunImportCommand);
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
            foreach (string file in System.IO.Directory.EnumerateFiles(Config.LogFileDirectory))
            {
                ImportFile importFile = new(file);
                SetImportFileDomain(importFile);
                SetImportFileStatus(importFile);
                if (importFile.Status != ImportFile.StatusImported)
                {
                    ImportFiles.Add(importFile);
                }
            }
        }

        private void SetImportFileDomain(ImportFile importFile)
        {
            foreach(DomainRow domain in DatabaseController.Instance.GetTable<DomainTable>().EnumerateAll())
            {
                if (importFile.DisplayName.StartsWith(domain.Preface, StringComparison.InvariantCulture))
                {
                    importFile.DomainId = domain.Id;
                    importFile.Domain = domain.DisplayName;
                    return;
                }
            }
        }

        private void SetImportFileStatus(ImportFile importFile)
        {
            if (importFile.DomainId == ImportFile.UninitializedDomaindId)
            {
                importFile.Status = ImportFile.StatusIneligible;
                return;
            }

            ImportFileRow logFileRow = DatabaseController.Instance.GetTable<ImportFileTable>().GetSingleRecord(importFile.DomainId, importFile.DisplayName);
            importFile.Status = logFileRow == null ? ImportFile.StatusReady : ImportFile.StatusImported;
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
                    DatabaseController.Instance.GetTable<RefererTable>(),
                    DatabaseController.Instance.GetTable<RequestTable>(),
                    DatabaseController.Instance.GetTable<UserAgentTable>()
                };

                DatabaseController.Instance.Transaction.ExecuteTransaction((transaction) =>
                {
                    LogEntryProcessor.Init();
                    List<ImportFile> processed = new List<ImportFile>();

                    foreach (ImportFile logFile in ImportFiles.Where(lf => lf.Status == ImportFile.StatusReady))
                    {
                        string[] lines = System.IO.File.ReadAllLines(logFile.Path);
                        ImportFileRow import = importTable.Create(logFile.DisplayName, logFile.DomainId, lines.LongLength);
                        long lineNumber = 0;
                        foreach (string line in lines)
                        {
                            LogEntry entry = LineParser.ParseLine(line, logFile.DomainId, import.Id, lineNumber);
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
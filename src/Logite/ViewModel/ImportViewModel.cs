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
    /// Provides the logic that is used to display and import the raw log files.
    /// </summary>
    public class ImportViewModel : DataGridViewModel<ImportFileTable>
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
                if (importFile.FileName.StartsWith(domain.Preface, StringComparison.InvariantCulture))
                {
                    importFile.Domain = domain;
                    return;
                }
            }
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
                        //DemandDomainController.Instance.Load(logFile.Domain);
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
using Restless.Logite.Core;
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using Restless.Logite.Resources;
using Restless.Toolkit.Controls;
using Restless.Toolkit.Mvvm.Collections;
using System;
using System.ComponentModel;
using System.Data;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Provides the logic that is used to display opening information.
    /// </summary>
    public class StartViewModel : DataGridViewModel<DomainTable>
    {
        #region Private
        private int userDomainCount;
        private string databaseDirectory;
        private DomainRow domain;
        #endregion

        /************************************************************************/

        #region Properties
        public string DatabaseDirectory
        {
            get => databaseDirectory;
            private set => SetProperty(ref databaseDirectory, value);
        }

        public int UserDomainCount
        {
            get => userDomainCount;
            set => SetProperty(ref userDomainCount, value);
        }

        public string LogFileDirectory
        {
            get => Config.LogFileDirectory;
            private set
            {
                Config.LogFileDirectory = value;
                OnPropertyChanged();
            }
        }

        public DomainRow Domain
        {
            get => domain;
            private set => SetProperty(ref domain, value);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StartViewModel"/> class.
        /// </summary>
        public StartViewModel() : base()
        {
            DisplayName = Strings.HeaderStartView;
            DatabaseDirectory = AppInfo.DatabaseRootFolder;

            Columns.Create("Id", DomainTable.Defs.Columns.Id).MakeFixedWidth(FixedWidth.W032);
            Columns.Create("Name", DomainTable.Defs.Columns.DisplayName);
            Columns.Create("Preface", DomainTable.Defs.Columns.Preface);

            Commands.Add("ConfigureLogDirectory", RunConfigureLogDirectoryCommand);
            Commands.Add("ConfigureDatabaseDirectory", RunConfigureDatabaseDirectoryCommand);
            Commands.Add("AddDomain", RunAddDomainCommand);
            Commands.Add("RemoveDomain", RunRemoveDomainCommand);

            UpdateUserDomainCount();
        }
        #endregion

        /************************************************************************/

        #region Protected Methods
        /// <summary>
        /// Called when the selected domain item changes.
        /// </summary>
        protected override void OnSelectedItemChanged()
        {
            if (SelectedDataRow != null)
            {
                Domain = new DomainRow(SelectedDataRow);
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void RunConfigureLogDirectoryCommand(object parm)
        {
            try
            {
                Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new()
                {
                    SelectedPath = LogFileDirectory
                };

                if (dialog.ShowDialog() == true)
                {
                    LogFileDirectory = dialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageWindow.ShowError(ex.Message);
            }
        }

        private void RunConfigureDatabaseDirectoryCommand(object parm)
        {
            try
            {
                Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new()
                {
                    SelectedPath = DatabaseDirectory
                };

                if (dialog.ShowDialog() == true)
                {
                    DatabaseDirectory = dialog.SelectedPath;
                    RegistryManager.SetDatabaseDirectory(DatabaseDirectory);
                }
            }
            catch (Exception ex)
            {
                MessageWindow.ShowError(ex.Message);
            }
        }

        private void RunAddDomainCommand(object parm)
        {
            try
            {
                if (MessageWindow.ShowContinueCancel(Strings.TextAddDomain))
                {
                    Table.Create();
                    Refresh();
                    UpdateUserDomainCount();
                }
            }
            catch (Exception ex)
            {
                MessageWindow.ShowError(ex.Message);
            }
        }

        private void RunRemoveDomainCommand(object parm)
        {
            
        }

        private void UpdateUserDomainCount()
        {
            UserDomainCount = Table.Rows.Count - 1;
        }

        #endregion
    }
}
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using System;
using System.Windows;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Provides configuration services for the application.
    /// </summary>
    public class Config : Toolkit.Core.Database.SQLite.KeyValueTableBase
    {
        #region Public fields
        /// <summary>
        /// Provides static values for the main navigation pane.
        /// </summary>
        public static class MainNavigation
        {
            /// <summary>
            /// Gets the minimum width of the main navigation pane.
            /// </summary>
            public const double MinWidth = 120.0;

            /// <summary>
            /// Gets the maximum width of the main navigation pane.
            /// </summary>
            public const double MaxWidth = 292.0;

            /// <summary>
            /// Gets the default width of the main navigation pane.
            /// </summary>
            public const double DefaultWidth = 150.0;
        }

        /// <summary>
        /// Provides static values for detail panels.
        /// </summary>
        public static class DetailPanel
        {
            /// <summary>
            /// Gets the minumum open width of the domain detail panel.
            /// </summary>
            public const double DomainMinWidth = 302;

            /// <summary>
            /// Gets the maximum open width of the domain detail panel.
            /// </summary>
            public const double DomainMaxWidth = 426;

            /// <summary>
            /// Gets the default open width of the domain detail panel.
            /// </summary>
            public const double DomainDefaultWidth = 380;

            /// <summary>
            /// Gets the minumum open width of the alias detail panel.
            /// </summary>
            public const double AliasMinWidth = 302;

            /// <summary>
            /// Gets the maximum open width of the alias detail panel.
            /// </summary>
            public const double AliasMaxWidth = 426;

            /// <summary>
            /// Gets the default open width of the alias detail panel.
            /// </summary>
            public const double AliasDefaultWidth = 380;
        }

        /// <summary>
        /// Provides static default values for properties
        /// </summary>
        public static class Default
        {
            /// <summary>
            /// Provides static default values for the main window.
            /// </summary>
            public static class MainWindow
            {
                /// <summary>
                /// Gets the default width for the main window.
                /// </summary>
                public const int Width = 1500;

                /// <summary>
                /// Gets the default height for the main window.
                /// </summary>
                public const int Height = 860;

                /// <summary>
                /// Gets the minimum width for the main window.
                /// </summary>
                public const int MinWidth = 840;

                /// <summary>
                /// Gets the minimum height for the main window.
                /// </summary>
                public const int MinHeight = 500;
            }

            /// <summary>
            /// Provides static default property values for DataGrid
            /// </summary>
            public static class DataGrid
            {
                /// <summary>
                /// Gets the default value for data grid row height.
                /// </summary>
                public const int RowHeight = 32;

                /// <summary>
                /// Gets the default value for data grid alternation count;
                /// </summary>
                public const int AlternationCount = 2;

                /// <summary>
                /// Gets the minimum value for data grid alternation count.
                /// </summary>
                public const int MinAlternationCount = 2;

                /// <summary>
                /// Gets the maximum value for data grid alternation count.
                /// </summary>
                public const int MaxAlternationCount = 5;

            }
        }

        #endregion

        /************************************************************************/

        #region Window settings
        /// <summary>
        /// Gets or sets the width of the main window
        /// </summary>
        public int MainWindowWidth
        {
            get => GetItem(Default.MainWindow.Width);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets the height of the main window
        /// </summary>
        public int MainWindowHeight
        {
            get => GetItem(Default.MainWindow.Height);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets the state of the main window
        /// </summary>
        public WindowState MainWindowState
        {
            get => (WindowState)GetItem((int)WindowState.Normal);
            set => SetItem((int)value);
        }
        #endregion

        /************************************************************************/

        #region Navigator state
        /// <summary>
        /// Gets or sets the width of the main navigation panel.
        /// </summary>
        public int MainNavigationWidth
        {
            get => GetItem((int)MainNavigation.DefaultWidth);
            set => SetItem(value);
        }

        ///// <summary>
        ///// Gets or sets whether the domain detail panel is visible.
        ///// </summary>
        //public bool DomainDetailVisible
        //{
        //    get => GetItem(false);
        //    set => SetItem(value);
        //}

        ///// <summary>
        ///// Gets or sets the domain detail width.
        ///// </summary>
        //public int DomainDetailWidth
        //{
        //    get => GetItem((int)DetailPanel.DomainDefaultWidth);
        //    set => SetItem(value);
        //}

        ///// <summary>
        ///// Gets or sets the domain filter id.
        ///// </summary>
        //public int DomainFilterId
        //{
        //    get => GetItem(0);
        //    set => SetItem(value);
        //}

        ///// <summary>
        ///// Gets or sets whether the alias detail panel is visible.
        ///// </summary>
        //public bool AliasDetailVisible
        //{
        //    get => GetItem(false);
        //    set => SetItem(value);
        //}

        ///// <summary>
        ///// Gets or sets the alias detail width.
        ///// </summary>
        //public int AliasDetailWidth
        //{
        //    get => GetItem((int)DetailPanel.AliasDefaultWidth);
        //    set => SetItem(value);
        //}

        ///// <summary>
        ///// Gets or sets the alias filter id.
        ///// </summary>
        //public int AliasFilterId
        //{
        //    get => GetItem(0);
        //    set => SetItem(value);
        //}

        /// <summary>
        /// Gets or sets whether the services navigator is expanded.
        /// </summary>
        public bool NavServicesExpander
        {
            get => GetItem(false);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets whether the analysis navigator is expanded.
        /// </summary>
        public bool NavAnalysisExpander
        {
            get => GetItem(false);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets whether the tools navigator is expanded.
        /// </summary>
        public bool NavToolsExpander
        {
            get => GetItem(false);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets a boolean that determines if the getting started sceen appears at startup.
        /// </summary>
        public bool NavigateStart
        {
            get => GetItem(true);
            set => SetItem(value);
        }
        #endregion

        /************************************************************************/

        #region Formatting options
        /// <summary>
        /// Gets the date format for the application.
        /// </summary>
        public string DateFormat
        {
            get => GetItem("yyyy-MM-dd");
            set
            {
                SetItem(value);
                Toolkit.Core.Default.Format.Date = value;
                //Controls.PopupCalendar.SetDateFormat(value);
            }
        }
        #endregion

        /************************************************************************/

        #region Other
        /// <summary>
        /// Gets or sets the root directory where log files are stored.
        /// </summary>
        public string LogFileDirectory
        {
            get => GetItem(string.Empty);
            set => SetItem(value);
        }
        #endregion

        /************************************************************************/

        #region Configuration state
        /// <summary>
        /// Gets or sets the selected configuration section.
        /// </summary>
        public long SelectedConfigSection
        {
            get => GetItem(1);
            set => SetItem(value);
        }
        #endregion
        
        /************************************************************************/

        #region Static singleton access and constructor
        /// <summary>
        /// Gets the singleton instance of this class
        /// </summary>
        public static Config Instance { get; } = new Config();

        private Config() : base(DatabaseController.Instance.GetTable<ConfigTable>())
        {
            Toolkit.Core.Default.Format.Date = DateFormat;
        }

        /// <summary>
        /// Static constructor. Tells C# compiler not to mark type as beforefieldinit.
        /// </summary>
        static Config()
        {
        }
        #endregion

        /************************************************************************/

        /// <summary>
        /// Sets the starting value of <see cref="LogFileDirectory"/>.
        /// If it is already set, this method does nothing.
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetDefaultLogDirectory(string value)
        {
            if (value == null || value.Length == 0)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (LogFileDirectory.Length == 0)
            {
                LogFileDirectory = value;
            }
        }


        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when a configuration value changes. Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyId"></param>
        protected override void OnRowValueChanged(string propertyId)
        {

        }
        #endregion
    }
}
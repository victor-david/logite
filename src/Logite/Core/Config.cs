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
            /// Gets the minumum open width of the log entry detail panel.
            /// </summary>
            public const double LogEntryMinWidth = 292;

            /// <summary>
            /// Gets the maximum open width of the log entry detail panel.
            /// </summary>
            public const double LogEntryMaxWidth = 496;

            /// <summary>
            /// Gets the default open width of the log entry detail panel.
            /// </summary>
            public const double LogEntryDefaultWidth = 360;
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

            public static class Format
            {
                public const string AppDate = "yyyy-MM-dd";

                public const string LogDate = "dd/MMM/yyyy:HH:mm:ss zzz";

                public const string LogCulture = "en-us";

                public const string DisplayDate = "yyyy-MMM-dd HH:mm:ss";
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

        #region Panels
        /// <summary>
        /// Gets or sets a boolean value that determines if the log entry detail panel is visible.
        /// </summary>
        public bool LogEntryDetailVisible
        {
            get => GetItem(false);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets the log entry detail width.
        /// </summary>
        public int LogEntryDetailWidth
        {
            get => GetItem((int)DetailPanel.LogEntryDefaultWidth);
            set => SetItem(value);
        }
        #endregion

        /************************************************************************/

        #region Formatting options
        /// <summary>
        /// Gets or sets the date format for the application.
        /// </summary>
        public string DateFormat
        {
            get => GetItem(Default.Format.AppDate);
            set
            {
                SetItem(value);
                Toolkit.Core.Default.Format.Date = value;
                //Controls.PopupCalendar.SetDateFormat(value);
            }
        }

        /// <summary>
        /// Gets or sets the date format used to parse the date portion of a log line.
        /// </summary>
        public string LogLineDateFormat
        {
            get => GetItem(Default.Format.LogDate);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets the culture identifier used to parse the date portion of a log line.
        /// </summary>
        public string LogLineCulture
        {
            get => GetItem(Default.Format.LogCulture);
            set => SetItem(value);
        }

        public string LogDisplayFormat
        {
            get => GetItem(Default.Format.DisplayDate);
            set => SetItem(value);
        }
        #endregion

        /************************************************************************/

        #region Ftp / Import settings
        /// <summary>
        /// Gets or sets the ftp host.
        /// </summary>
        public string FtpHost
        {
            get => GetItem(string.Empty);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets the ftp user name.
        /// </summary>
        public string FtpUserName
        {
            get => GetItem(string.Empty);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets the full path to the ftp key file.
        /// </summary>
        public string FtpKeyFile
        {
            get => GetItem(string.Empty);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets the remote log directory.
        /// </summary>
        public string RemoteLogDirectory
        {
            get => GetItem(string.Empty);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets the root directory where log files are stored.
        /// </summary>
        public string LocalLogDirectory
        {
            get => GetItem(string.Empty);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets a boolean value that determines if a proposed download file 
        /// may overwrite an existing file.
        /// </summary>
        public bool OverwriteLogFiles
        {
            get => GetItem(false);
            set => SetItem(value);
        }

        /// <summary>
        /// Gets or sets a regular expression that is used to determine if a proposed 
        /// download file is eligible for download
        /// </summary>
        public string LogFileRegex
        {
            get => GetItem(@"\d{8}$");
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
        /// Sets the starting value of <see cref="LocalLogDirectory"/>.
        /// If it is already set, this method does nothing.
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetDefaultLocalLogDirectory(string value)
        {
            if (value == null || value.Length == 0)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (LocalLogDirectory.Length == 0)
            {
                LocalLogDirectory = value;
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
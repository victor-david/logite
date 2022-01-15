using Restless.Logite.Core;
using Restless.Logite.Resources;
using Restless.Toolkit.Controls;
using Restless.Toolkit.Mvvm;
using System.Collections.ObjectModel;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Represents a single file in the log import directory
    /// </summary>
    public class LogFile : ObservableObject
    {
        #region Private
        private string domain;
        private string status;
        #endregion


        /************************************************************************/

        #region Properties
        public const string StatusReady = "Ready";
        public const string StatusImported = "Imported";
        public const string StatusIneligible = "Ineligible";
        public const long UninitializedDomaindId = -1;

        /// <summary>
        /// Gets the full path to the file
        /// </summary>
        public string Path
        {
            get;
        }

        /// <summary>
        /// Gets the display name (file only, without path)
        /// </summary>
        public string DisplayName
        {
            get;
        }

        /// <summary>
        /// Gets or sets the domain id associated with this file.
        /// </summary>
        public long DomainId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the domain display name
        /// </summary>
        public string Domain
        {
            get => domain;
            set => SetProperty(ref domain, value);
        }

        /// <summary>
        /// Gets or sets the file status
        /// </summary>
        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="LogFile"/> class.
        /// </summary>
        /// <param name="path"></param>
        public LogFile(string path)
        {
            Path = path;
            DisplayName = System.IO.Path.GetFileName(path);
            DomainId = UninitializedDomaindId;
        }
        #endregion

        public override string ToString()
        {
            return $"{DisplayName}, Domain id: {DomainId}";
        }
    }
}
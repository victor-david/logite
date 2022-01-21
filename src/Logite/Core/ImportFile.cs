using Restless.Logite.Database.Tables;
using Restless.Toolkit.Mvvm;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Represents a single file in the log import directory
    /// </summary>
    public class ImportFile : ObservableObject
    {
        #region Private
        private string status;
        #endregion

        /************************************************************************/

        #region Properties
        public const string StatusReady = "Ready";
        public const string StatusImported = "Imported";
        public const string StatusIneligible = "Ineligible";

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
        public string FileName
        {
            get;
        }

        /// <summary>
        /// Gets or sets the domain associated with this import file.
        /// </summary>
        public DomainRow Domain
        {
            get;
            set;
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
        /// Creates a new instance of the <see cref="ImportFile"/> class.
        /// </summary>
        /// <param name="path"></param>
        public ImportFile(string path)
        {
            Path = path;
            FileName = System.IO.Path.GetFileName(path);
        }
        #endregion

        public override string ToString()
        {
            return $"{FileName}, Domain id: {Domain?.Id}";
        }
    }
}
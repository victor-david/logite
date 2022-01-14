using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Utility;
using System;

namespace Restless.Logite.Core
{
    /// <summary>
    /// A singleton class that provides information about the application.
    /// </summary>
    public sealed class ApplicationInfo
    {
        #region Public properties
        /// <summary>
        /// Gets the assembly information object.
        /// </summary>
        public AssemblyInfo Assembly
        {
            get;
        }

        /// <summary>
        /// Gets the root folder for the database. The user can change the location,
        /// but it does not take effect until the application is restarted.
        /// </summary>
        /// <remarks>
        public string DatabaseRootFolder
        {
            get => DatabaseController.Instance.DatabaseRoot;
        }

        /// <summary>
        /// Gets a boolean value that indicates if the current process is a 64 bit process.
        /// </summary>
        public bool Is64Bit
        {
            get => Environment.Is64BitProcess;
        }
        #endregion

        /************************************************************************/

        #region Singleton access and constructors
        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static ApplicationInfo Instance { get; } = new ApplicationInfo();

        private ApplicationInfo()
        {
            Assembly = new AssemblyInfo(AssemblyInfoType.Entry);
        }

        /// <summary>
        /// Static constructor. Tells C# compiler not to mark type as beforefieldinit.
        /// </summary>
        static ApplicationInfo()
        {
        }
        #endregion
    }
}
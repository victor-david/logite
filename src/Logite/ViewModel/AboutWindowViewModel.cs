using Restless.Logite.Core;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's about window.
    /// </summary>
    public class AboutWindowViewModel : ApplicationViewModel
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindowViewModel"/> class.
        /// </summary>
        public AboutWindowViewModel()
        {
#if DEBUG
            DisplayName = $"About {AppInfo.Assembly.Product} {AppInfo.Assembly.VersionMajor} (DEBUG)";
#else
            DisplayName = $"About {AppInfo.Assembly.Product} {AppInfo.Assembly.VersionMajor}";
#endif
        }
        #endregion
    }
}
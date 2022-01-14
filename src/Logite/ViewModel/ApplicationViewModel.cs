using Restless.Logite.Controls;
using Restless.Logite.Core;
using Restless.Toolkit.Mvvm;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Provides functionality common to all view models. This class must be inherited.
    /// </summary>
    public abstract class ApplicationViewModel : ViewModelBase, INavigator
    {
        #region Public properties
        /// <summary>
        /// Gets the singleton instance of the configuration object. 
        /// Although derived classes can access the singleton instance directly,
        /// this enables easy binding to certain configuration properties
        /// </summary>
        public static Core.Config Config => Core.Config.Instance;

        /// <summary>
        /// Gets the application information object.
        /// </summary>
        public static ApplicationInfo AppInfo => ApplicationInfo.Instance;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationViewModel"/> class.
        /// </summary>
        protected ApplicationViewModel() : base()
        {
        }
        #endregion
    }
}
namespace Restless.Logite.Core
{
    /// <summary>
    /// Represents a single time filter item.
    /// </summary>
    public class TimeFilterItem
    {
        #region Public properties
        /// <summary>
        /// Gets the name of the filter item
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Gets the time filter value for this item.
        /// </summary>
        public TimeFilter Filter
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeFilterItem"/> class.
        /// </summary>
        /// <param name="name">Display name of this item.</param>
        /// <param name="filter">The register preset filter associated with this item.</param>
        public TimeFilterItem(string name, TimeFilter filter)
        {
            Name = name;
            Filter = filter;
        }
        #endregion
    }
}
using System;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Represents arguments for the <see cref="FilterController.FilterChanged"/> event.
    /// </summary>
    public class FilterChangedEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// Gets the time filter item.
        /// </summary>
        public TimeFilterItem Item
        {
            get;
        }

        /// <summary>
        /// Gets the filter text.
        /// </summary>
        public string FilterText
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterChangedEventArgs"/> class.
        /// </summary>
        /// <param name="timeFilter">Selected time filter item.</param>
        /// <param name="filterText">Filter text</param>
        public FilterChangedEventArgs(TimeFilterItem timeFilter, string filterText)
        {
            Item = timeFilter;
            FilterText = filterText;
        }
        #endregion
    }
}
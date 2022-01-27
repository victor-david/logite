using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Represents a data point that holds a single count.
    /// </summary>
    public class CountDataPoint : DataPoint
    {
        #region Properties
        /// <summary>
        /// Gets the count
        /// </summary>
        public long Count
        {
            get;
            internal set;
        }
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Creates a new instance of the <see cref="CountDataPoint"/> class.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The newly created object.</returns>
        internal static CountDataPoint Create(DateTime date)
        {
            return new CountDataPoint(date);
        }

        private CountDataPoint(DateTime date): base (date)
        {
            Count = 0;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets a string representation of this object.
        /// </summary>
        /// <returns>A string that describes this object</returns>
        public override string ToString()
        {
            return $"Data Point: {Date} {Count}";
        }
        #endregion
    }
}
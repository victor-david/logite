using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Base class for a data point
    /// </summary>
    public abstract class DataPoint
    {
        /// <summary>
        /// Gets the date (normalized, no time portion)
        /// </summary>
        public DateTime Date
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint"/> class.
        /// </summary>
        /// <param name="date">The date for the data point</param>
        protected DataPoint(DateTime date)
        {
            Date = new DateTime(date.Year, date.Month, date.Day);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Base class for a data point
    /// </summary>
    public class StatusDataPoint : DataPoint
    {
        #region Properties
        /// <summary>
        /// Gets the count of 200 responses.
        /// </summary>
        public long Count200
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the count of 302 responses.
        /// </summary>
        public long Count302
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the count of 304 responses.
        /// </summary>
        public long Count304
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the count of 400 responses.
        /// </summary>
        public long Count400
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the count of 404 responses.
        /// </summary>
        public long Count404
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the count of 444 responses.
        /// </summary>
        public long Count444
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the count of 500 responses.
        /// </summary>
        public long Count500
        {
            get;
            internal set;
        }
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Creates a new instance of the <see cref="StatusDataPoint"/> class.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The newly created object.</returns>
        internal static StatusDataPoint Create(DateTime date)
        {
            return new StatusDataPoint(date);
        }

        private StatusDataPoint(DateTime date): base (date)
        {
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
            return $"Data Point: {Date} 200:{Count200} 400:{Count400} 404:{Count404} 444:{Count444}";
        }
        #endregion
    }
}
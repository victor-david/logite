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
        private Dictionary<long, long> storage;
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
            storage = new Dictionary<long, long>();
            foreach (StatusCode.StatusCodeItem item in StatusCode.EnumerateAll())
            {
                storage.Add(item.Value, 0);
            }
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets the count for the specified status code.
        /// </summary>
        /// <param name="status">The status code</param>
        /// <returns>The count for the status</returns>
        public long GetCountForStatus(long status)
        {
            if (storage.ContainsKey(status))
            {
                return storage[status];
            }
            return 0;
        }

        /// <summary>
        /// Gets a string representation of this object.
        /// </summary>
        /// <returns>A string that describes this object</returns>
        public override string ToString()
        {
            return 
                $"Data Point: {Date} " +
                $"200:{storage[StatusCode.Code200.Value]} 400:{storage[StatusCode.Code400.Value]} " +
                $"404:{storage[StatusCode.Code404.Value]} 444:{storage[StatusCode.Code444.Value]}";
        }

        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Increments the count for the specified status code if it exists.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        internal void IncrementStatusCount(long statusCode)
        {
            if (storage.ContainsKey(statusCode))
            {
                storage[statusCode]++;
            }
        }
        #endregion
    }
}
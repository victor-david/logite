using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Represents a collection of <see cref="DataPoint"/> objects.
    /// </summary>
    public class DataPointCollection<T> : IEnumerable<T> where T: DataPoint
    {
        #region Private
        private List<T> storage;
        #endregion

        /************************************************************************/

        #region Constructor (internal)
        /// <summary>
        /// Initializes a new instance of the <see cref="DataPointCollection"/> class.
        /// </summary>
        internal DataPointCollection()
        {
            storage = new List<T>();
        }
        #endregion

        /************************************************************************/

        #region IEnumerable interface

        public IEnumerator<T> GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return storage.GetEnumerator();
        }
        #endregion

        /************************************************************************/

        #region Internal / private methods
        /// <summary>
        /// Adds the specified data point to the collection if it doesn't already exist
        /// </summary>
        /// <param name="point">The data point</param>
        /// <returns>
        /// If <paramref name="point"/> exists according to its date, returns
        /// the existing object; otherwise, returns <paramref name="point"/>
        /// </returns>
        internal T Add(T point)
        {
            if (point == null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            T existing = Get(point.Date);
            if (existing != null)
            {
                return existing;
            }

            storage.Add(point);
            return point;
        }

        private T Get(DateTime normalizedDate)
        {
            foreach (T point in this)
            {
                if (point.Date == normalizedDate)
                {
                    return point;
                }
            }
            return null;
        }
        #endregion
    }
}
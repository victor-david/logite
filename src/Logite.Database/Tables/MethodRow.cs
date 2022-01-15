using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Data;
using Columns = Restless.Logite.Database.Tables.MethodTable.Defs.Columns;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Encapsulates a single row from the <see cref="MethodTable"/>.
    /// </summary>
    public class MethodRow : RowObjectBase<MethodTable>
    {
        #region Public properties
        /// <summary>
        /// Gets the id for this row object.
        /// </summary>
        public long Id
        {
            get => GetInt64(Columns.Id);
        }

        /// <summary>
        /// Gets the method name.
        /// </summary>
        public string Method
        {
            get => GetString(Columns.Method);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodRow"/> class.
        /// </summary>
        /// <param name="row">The data row</param>
        public MethodRow(DataRow row) : base(row)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets the string representation of this object.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return Method;
        }
        #endregion
    }
}
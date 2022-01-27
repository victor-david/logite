using Columns = Restless.Logite.Database.Tables.StatusTable.Defs.Columns;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Encapsulates a single row from the <see cref="StatusTable"/>.
    /// </summary>
    public class StatusRow : RawRow
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
        /// Gets the status
        /// </summary>
        public long Status
        {
            get => GetInt64(Columns.Status);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusRow"/> class.
        /// </summary>
        public StatusRow() : base(StatusTable.Defs.TableName)
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
            return $"Status: {Status}";
        }
        #endregion
    }
}
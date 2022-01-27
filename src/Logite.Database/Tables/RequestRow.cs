using Columns = Restless.Logite.Database.Tables.RequestTable.Defs.Columns;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Encapsulates a single row from the <see cref="RequestTable"/>.
    /// </summary>
    public class RequestRow : RawRow
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
        /// Gets the request name.
        /// </summary>
        public string Request
        {
            get => GetString(Columns.Request);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestRow"/> class.
        /// </summary>
        public RequestRow() : base(RequestTable.Defs.TableName)
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
            return Request;
        }
        #endregion
    }
}
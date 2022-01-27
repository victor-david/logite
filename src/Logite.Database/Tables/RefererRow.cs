using Columns = Restless.Logite.Database.Tables.RefererTable.Defs.Columns;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Encapsulates a single row from the <see cref="RefererTable"/>.
    /// </summary>
    public class RefererRow : RawRow
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
        /// Gets the referer name.
        /// </summary>
        public string Referer
        {
            get => GetString(Columns.Referer);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RefererRow"/> class.
        /// </summary>
        public RefererRow() : base(RefererTable.Defs.TableName)
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
            return Referer;
        }
        #endregion
    }
}
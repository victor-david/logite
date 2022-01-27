using Columns = Restless.Logite.Database.Tables.UserAgentTable.Defs.Columns;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Encapsulates a single row from the <see cref="UserAgentTable"/>.
    /// </summary>
    public class UserAgentRow : RawRow
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
        /// Gets the user agent name.
        /// </summary>
        public string UserAgent
        {
            get => GetString(Columns.Agent);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentRow"/> class.
        /// </summary>
        public UserAgentRow() : base(UserAgentTable.Defs.TableName)
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
            return UserAgent;
        }
        #endregion
    }
}
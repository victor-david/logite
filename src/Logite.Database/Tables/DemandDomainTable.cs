using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Represents a table that has a domain id and is loaded upon demand for the specified domain
    /// </summary>
    public abstract class DemandDomainTable : Core.ApplicationTableBase
    {
        #region Public fields
        /// <summary>
        /// The name of the unique id column. Derived classes must use this name.
        /// </summary>
        public const string IdColumnName = DefaultPrimaryKeyName;

        /// <summary>
        /// The name of the domain id column. Derived classes must use this name.
        /// </summary>
        public const string DomainIdColumnName = "domainid";
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DemandDomainTable"/> class.
        /// </summary>
        protected DemandDomainTable(string tableName) : base(tableName)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Loads the data from the database into the Data collection for this table.
        /// </summary>
        /// <remarks>
        /// This method satisfies the abstract base class, but does not load any data.
        /// Data is loaded upon demand using the <see cref="Load(long)"/> method.
        /// </remarks>
        public override void Load()
        {
            Load("0=1", IdColumnName);
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Loads data according to the specified domain id.
        /// </summary>
        /// <param name="domainId">The domain id</param>
        internal void Load(long domainId)
        {
            Clear();
            string sql = $"SELECT * FROM {Namespace}.{TableName} WHERE {DomainIdColumnName}={domainId}";

            using (var selectCommand = new SQLiteCommand(Controller.Connection))
            {
                selectCommand.CommandText = sql;
                Load(selectCommand.ExecuteReader());
            }
        }
        #endregion
    }
}

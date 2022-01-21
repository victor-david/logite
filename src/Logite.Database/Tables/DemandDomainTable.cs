using Restless.Logite.Database.Core;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Represents a table that is loaded upon demand for the specified domain.
    /// Tables that derive from this class use direct execution for various operations.
    /// </summary>
    public abstract class DemandDomainTable : Core.ApplicationTableBase
    {
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
            Load("0=1", PrimaryKeyName);
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Selects an id value using direct execution.
        /// </summary>
        /// <param name="columnName">The field name</param>
        /// <param name="columnValue">The file value</param>
        /// <returns>The id, or -1 if not found.</returns>
        protected long SelectScalarId(string columnName, string columnValue)
        {
            string sql = $"select {PrimaryKeyName} from {Namespace}.{TableName} where {columnName}='{columnValue}'";
            object result = Controller.Execution.Scalar(sql);
            if (result is long id)
            {
                return id;
            }
            return -1;
        }

        /// <summary>
        /// Inserts a row using direct execution
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <param name="columnValue">The column value</param>
        /// <returns>The id of the newly inserted row</returns>
        /// <exception cref="Exception">After insertion, the id could not be retreived</exception>
        protected long InsertScalarId(string columnName, string columnValue)
        {
            string sql = $"insert into {Namespace}.{TableName} ({columnName}) values('{columnValue}')";
            Controller.Execution.NonQuery(sql);
            long id = SelectScalarId(columnName, columnValue);
            if (id == -1)
            {
                throw new Exception($"{TableName}. Could not retrieve the id");
            }
            return id;
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        internal void Load(IdCollection ids)
        {
            Clear();
            string sql = $"SELECT * FROM {Namespace}.{TableName} WHERE {PrimaryKeyName} IN ({ids})";
            Load(Controller.Execution.Query(sql));
        }

        /// <summary>
        /// Inserts an entry if it doesn't exist.
        /// </summary>
        /// <param name="entry">The entry</param>
        /// <returns>The newly inserted id, or the existing id</returns>
        /// <remarks>
        /// Derived classes must override this method if they need this functionality.
        /// The base method throws a NotImplementedException.
        /// </remarks>
        internal virtual long InsertEntryIf(LogEntry entry)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

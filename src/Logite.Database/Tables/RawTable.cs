using Restless.Logite.Database.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Represents a table that is loaded upon demand for the specified domain.
    /// Tables that derive from this class use direct execution for various operations.
    /// </summary>
    public abstract class RawTable<T> : Core.ApplicationTableBase where T: RawRow
    {
        /// <summary>
        /// Gets the collection of raw rows
        /// </summary>
        public ObservableCollection<T> RawRows
        {
            get;
        }

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RawTable"/> class.
        /// </summary>
        protected RawTable(string tableName) : base(tableName)
        {
            RawRows = new ObservableCollection<T>();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Loads the data from the database into the Data collection for this table.
        /// </summary>
        /// <remarks>
        /// This method satisfies the abstract base class, but does not load any data.
        /// Data is loaded upon demand using the <see cref="LoadFromSql(string, Func{IDataReader, T})"/> method.
        /// </remarks>
        public override sealed void Load()
        {
            Load("0=1", PrimaryKeyName);
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Provides an enumerable that returns all records from <see cref="RawRows"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> EnumerateAll()
        {
            foreach (var row in RawRows)
            {
                yield return row;
            }
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Loads <see cref="RawRows"/> from the specified sql string.
        /// </summary>
        /// <param name="sql">The sql to execute</param>
        /// <param name="rawBuilder">
        /// A callback that returns a raw row. Return null to skip the row.
        /// </param>
        protected void LoadFromSql(string sql, Func<IDataReader, T> rawBuilder)
        {
            RawRows.Clear();
            IDataReader reader = Controller.Execution.Query(sql);
            while (reader.Read())
            {
                T rawRow = rawBuilder(reader);
                if (rawRow != null)
                {
                    RawRows.Add(rawRow);
                }
            }
        }

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
        /// <summary>
        /// Loads records into <see cref="RawRows"/>
        /// </summary>
        /// <param name="domainId">The domain id</param>
        /// <param name="ids">The list of ids to load.</param>
        /// <remarks>
        /// This method is used to load secondary tables (ipaddress, request, referer, etc.)
        /// that have a relationship with the primary table <see cref="LogEntryTable"/>.
        /// Derived classes must override this method if they need this functionality.
        /// The base method throws a NotImplementedException.
        /// </remarks>
        internal virtual void Load(long domainId, IdCollection ids)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Clears <see cref="RawRows"/>.
        /// </summary>
        internal void ClearRaw()
        {
            RawRows.Clear();
        }
        #endregion
    }
}

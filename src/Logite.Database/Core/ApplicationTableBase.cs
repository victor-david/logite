﻿namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Represents the base class for application tables. This class must be inherited.
    /// </summary>
    public abstract class ApplicationTableBase : Toolkit.Core.Database.SQLite.ApplicationTableBase
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTableBase"/> class using <see cref="DatabaseController.MainDepotSchemaName"/>.
        /// </summary>
        /// <param name="tableName">The table name</param>
        protected ApplicationTableBase(string tableName) : base(DatabaseController.Instance, DatabaseController.MainDepotSchemaName, tableName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTableBase"/> class using the specified schema name.
        /// </summary>
        /// <param name="schemaName">The schema name.</param>
        /// <param name="tableName">The table name</param>
        protected ApplicationTableBase(string schemaName, string tableName) : base(DatabaseController.Instance, schemaName, tableName)
        {
        }
        #endregion
    }
}
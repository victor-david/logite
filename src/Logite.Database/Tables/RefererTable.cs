using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    public class RefererTable : Core.ApplicationTableBase
    {
        #region Public properties
        /// <summary>
        /// Provides static definitions for table properties such as column names and relation names.
        /// </summary>
        public static class Defs
        {
            /// <summary>
            /// Specifies the name of this table.
            /// </summary>
            public const string TableName = "referer";

            /// <summary>
            /// Provides static column names for this table.
            /// </summary>
            public static class Columns
            {
                /// <summary>
                /// The name of the id column. This is the table's primary key.
                /// </summary>
                public const string Id = DefaultPrimaryKeyName;

                /// <summary>
                /// The referer.
                /// </summary>
                public const string Referer = "referer";
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RefererTable"/> class.
        /// </summary>
        public RefererTable() : base(Defs.TableName)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Loads the data from the database into the Data collection for this table.
        /// </summary>
        public override void Load()
        {
            Load(null, Defs.Columns.Id);
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Gets the column definitions for this table.
        /// </summary>
        /// <returns>A <see cref="ColumnDefinitionCollection"/>.</returns>
        protected override ColumnDefinitionCollection GetColumnDefinitions()
        {
            return new ColumnDefinitionCollection()
            {
                { Defs.Columns.Id, ColumnType.Integer, true },
                { Defs.Columns.Referer, ColumnType.Text},
            };
        }
        #endregion
    }
}
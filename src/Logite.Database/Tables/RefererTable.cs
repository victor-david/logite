using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    public class RefererTable : RawTable<RefererRow>
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

                /// <summary>
                /// Provides static column names for columns that are calculated from other values.
                /// </summary>
                public class Calculated
                {
                    /// <summary>
                    /// Number of usages.
                    /// </summary>
                    public const string UsageCount = "CalcUsageCount";
                }
            }

            /// <summary>
            /// Provides static relation names.
            /// </summary>
            public static class Relations
            {
                /// <summary>
                /// The name of the relation that relates the <see cref="RefererTable"/> to the <see cref="LogEntryTable"/>.
                /// </summary>
                public const string ToLogEntry = "RefererToLogEntry";
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

        /// <inheritdoc/>
        protected override void SetDataRelations()
        {
            CreateParentChildRelation<LogEntryTable>(Defs.Relations.ToLogEntry, Defs.Columns.Id, LogEntryTable.Defs.Columns.RefererId);
        }

        /// <inheritdoc/>
        protected override void UseDataRelations()
        {
            string expr = string.Format("Count(Child({0}).{1})", Defs.Relations.ToLogEntry, LogEntryTable.Defs.Columns.Id);
            CreateExpressionColumn<long>(Defs.Columns.Calculated.UsageCount, expr);
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Inserts an entry if it doesn't exist.
        /// </summary>
        /// <param name="entry">The log entry</param>
        /// <returns>The newly inserted id, or the existing id</returns>
        internal override long InsertEntryIf(LogEntry entry)
        {
            long id = SelectScalarId(Defs.Columns.Referer, entry.Referer);
            return id != -1 ? id : InsertScalarId(Defs.Columns.Referer, entry.Referer);
        }
        #endregion
    }
}
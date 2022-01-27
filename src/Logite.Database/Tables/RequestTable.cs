using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Data;

namespace Restless.Logite.Database.Tables
{
    public class RequestTable : RawTable<RequestRow>
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
            public const string TableName = "request";

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
                /// The request.
                /// </summary>
                public const string Request = "request";

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
                /// The name of the relation that relates the <see cref="RequestTable"/> to the <see cref="LogEntryTable"/>.
                /// </summary>
                public const string ToLogEntry = "RequestToLogEntry";
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTable"/> class.
        /// </summary>
        public RequestTable() : base(Defs.TableName)
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
                { Defs.Columns.Request, ColumnType.Text },
            };
        }

        /// <inheritdoc/>
        protected override void SetDataRelations()
        {
            CreateParentChildRelation<LogEntryTable>(Defs.Relations.ToLogEntry, Defs.Columns.Id, LogEntryTable.Defs.Columns.RequestId);
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
            long id = SelectScalarId(Defs.Columns.Request, entry.Request);
            return id != -1 ? id : InsertScalarId(Defs.Columns.Request, entry.Request);
        }
        #endregion
    }
}
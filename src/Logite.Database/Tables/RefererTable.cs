using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    public class RefererTable : DemandDomainTable
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
                public const string Id = IdColumnName;

                /// <summary>
                /// Id of the domain.
                /// </summary>
                public const string DomainId = DomainIdColumnName;

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
                { Defs.Columns.DomainId, ColumnType.Integer, false, false, DomainTable.Defs.Values.DomainZeroId, IndexType.Index },
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
        /// Inserts a referer record if it doesn't yet exist.
        /// </summary>
        /// <param name="referer">The request</param>
        /// <returns>The newly inserted id, or the existing id</returns>
        internal long InsertIf(LogEntry entry)
        {
            /* Referer should never be empty. If it comes from the logs as empty (which is possible),
             * it gets set to LogEntry.EmptyEntryPlaceholder in the LogEntry.SetReferer(value)
             */
            if (string.IsNullOrEmpty(entry.Referer))
            {
                throw new ArgumentNullException(nameof(entry.Referer));
            }

            DataRow row = GetUniqueRow(Select($"{Defs.Columns.Referer}='{entry.Referer}' AND {Defs.Columns.DomainId}={entry.DomainId}"));
            if (row != null)
            {
                return (long)row[Defs.Columns.Id];
            }

            row = NewRow();
            row[Defs.Columns.DomainId] = entry.DomainId;
            row[Defs.Columns.Referer] = entry.Referer;
            Rows.Add(row);
            Save();
            return (long)row[Defs.Columns.Id];
        }
        #endregion
    }
}
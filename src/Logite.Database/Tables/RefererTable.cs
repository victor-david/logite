using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
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

            /// <summary>
            /// Provides static values.
            /// </summary>
            public static class Values
            {
                /// <summary>
                /// The id for the "No referer" entry.
                /// </summary>
                public const long RefererZeroId = 0;

                /// <summary>
                /// The name for the "No referer" entry.
                /// </summary>
                public const string RefererZeroName = "--";

                /// <summary>
                /// The name for an attack referer.
                /// </summary>
                public const string RefererAttackName = "Referer attack";
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

        /// <summary>
        /// Gets the referer id for the specified referer.
        /// </summary>
        /// <param name="referer">The method string</param>
        /// <returns>The referer id</returns>
        public long GetRefererId(string referer)
        {
            if (!string.IsNullOrEmpty(referer))
            {

            }
            return Defs.Values.RefererZeroId;
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

        /// <summary>
        /// Gets a list of column names to use in subsequent initial insert operations.
        /// These are used only when the table is empty, i.e. upon first creation.
        /// </summary>
        /// <returns>A list of column names</returns>
        protected override List<string> GetPopulateColumnList()
        {
            return new List<string>() { Defs.Columns.Id, Defs.Columns.Referer };
        }

        /// <summary>
        /// Provides an enumerable that returns values for each row to be populated.
        /// </summary>
        /// <returns>An IEnumerable</returns>
        protected override IEnumerable<object[]> EnumeratePopulateValues()
        {
            yield return new object[] { Defs.Values.RefererZeroId, Defs.Values.RefererZeroName };
        }

        /// <inheritdoc/>
        protected override void SetDataRelations()
        {
            CreateParentChildRelation<LogEntryTable>(Defs.Relations.ToLogEntry, Defs.Columns.Id, LogEntryTable.Defs.Columns.RefererId);
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Inserts a referer record if it doesn't yet exist.
        /// </summary>
        /// <param name="referer">The request</param>
        /// <returns>The newly inserted id, or the existing id</returns>
        internal long InsertIf(string referer)
        {
            if (!string.IsNullOrEmpty(referer))
            {
                DataRow row = GetUniqueRow(Select($"{Defs.Columns.Referer}='{referer}'"));
                if (row != null)
                {
                    return (long)row[Defs.Columns.Id];
                }

                row = NewRow();
                row[Defs.Columns.Referer] = referer;
                Rows.Add(row);
                Save();
                return (long)row[Defs.Columns.Id];
            }
            return Defs.Values.RefererZeroId;
        }
        #endregion
    }
}
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Represents the table that holds info about domain that are included in the log files.
    /// </summary>
    public partial class DomainTable : Core.ApplicationTableBase
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
            public const string TableName = "domain";

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
                /// The name of the domain for display.
                /// </summary>
                public const string DisplayName = "displayname";

                /// <summary>
                /// The preface of the log file that identifies this domain.
                /// </summary>
                public const string Preface = "preface";

                /// <summary>
                /// Date / time record created.
                /// </summary>
                public const string Created = "created";
            }

            /// <summary>
            /// Provides static relation names.
            /// </summary>
            public static class Relations
            {
                /// <summary>
                /// The name of the relation that relates the <see cref="DomainTable"/> to the <see cref="ImportFileTable"/>.
                /// </summary>
                public const string ToImportFile = "DomainToImportFile";

                /// <summary>
                /// The name of the relation that relates the <see cref="DomainTable"/> to the <see cref="LogEntryTable"/>.
                /// </summary>
                public const string ToLogEntry = "DomainToLogEntry";
            }

            /// <summary>
            /// Provides static values
            /// </summary>
            public static class Values
            {
                /// <summary>
                /// The id for the default domain.
                /// </summary>
                public const long DomainZeroId = 0;

                /// <summary>
                /// The display name for the default domain.
                /// </summary>
                public const string DomainZeroDisplayName = "Default";

                /// <summary>
                /// The preface for the default domain.
                /// </summary>
                public const string DomainZeroPreface = "default";

                /// <summary>
                /// The display name when adding a new domain
                /// </summary>
                public const string NewDomainDisplayName = "New Domain";

                /// <summary>
                /// The display preface when adding a new domain
                /// </summary>
                public const string NewDomainPreface = "xxx.access";
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainTable"/> class.
        /// </summary>
        public DomainTable() : base(Defs.TableName)
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
        /// Creates a record with default values.
        /// </summary>
        /// <returns>The newly added <see cref="DomainRow"/></returns>
        public DomainRow Create()
        {
            var obj = new DomainRow(NewRow())
            {
                DisplayName = Defs.Values.NewDomainDisplayName,
                Preface = Defs.Values.NewDomainPreface
            };
            obj.Row[Defs.Columns.Created] = DateTime.UtcNow;
            Rows.Add(obj.Row);
            Save();
            return obj;
        }

        /// <summary>
        /// Gets a single record identified by <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A row object, or null if none.</returns>
        public DomainRow GetSingleRecord(long id)
        {
            DataRow[] rows = Select($"{Defs.Columns.Id}={id}");
            if (rows.Length == 1)
            {
                return new DomainRow(rows[0]);
            }
            return null;
        }
        #endregion

        /************************************************************************/

        #region Public methods (enumeration)
        public IEnumerable<DomainRow> EnumerateAll()
        {
            foreach (var row in EnumerateRows(null, Defs.Columns.DisplayName))
            {
                yield return new DomainRow(row);
            }
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
                { Defs.Columns.DisplayName, ColumnType.Text, false, false},
                { Defs.Columns.Preface, ColumnType.Text, false, false },
                { Defs.Columns.Created, ColumnType.Timestamp },
            };
        }

        /// <inheritdoc/>
        protected override void SetDataRelations()
        {
            CreateParentChildRelation<ImportFileTable>(Defs.Relations.ToImportFile, Defs.Columns.Id, ImportFileTable.Defs.Columns.DomainId);
            CreateParentChildRelation<LogEntryTable>(Defs.Relations.ToLogEntry, Defs.Columns.Id, LogEntryTable.Defs.Columns.DomainId);
        }

        protected override void UseDataRelations()
        {
        }

        /// <summary>
        /// Gets a list of column names to use in subsequent initial insert operations.
        /// These are used only when the table is empty, i.e. upon first creation.
        /// </summary>
        /// <returns>A list of column names</returns>
        protected override List<string> GetPopulateColumnList()
        {
            return new List<string>() { Defs.Columns.Id, Defs.Columns.DisplayName, Defs.Columns.Preface, Defs.Columns.Created };
        }

        /// <summary>
        /// Provides an enumerable that returns values for each row to be populated.
        /// </summary>
        /// <returns>An IEnumerable</returns>
        protected override IEnumerable<object[]> EnumeratePopulateValues()
        {
            yield return new object[] { Defs.Values.DomainZeroId, Defs.Values.DomainZeroDisplayName, Defs.Values.DomainZeroPreface, DateTime.UtcNow };
#if DEBUG
            yield return new object[] { Defs.Values.DomainZeroId + 1, "Public", "public", DateTime.UtcNow };
            yield return new object[] { Defs.Values.DomainZeroId + 2, "User", "user", DateTime.UtcNow };
            yield return new object[] { Defs.Values.DomainZeroId + 3, "Kong", "kong", DateTime.UtcNow };
            yield return new object[] { Defs.Values.DomainZeroId + 4, "Service", "service", DateTime.UtcNow };

#endif
        }
        #endregion
    }
}
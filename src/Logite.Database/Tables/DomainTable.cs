using Restless.Logite.Database.Core;
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
                /// The currently selected display mode
                /// </summary>
                public const string DisplayMode = "mode";

                /// <summary>
                /// The number of past days for reporting on this domain
                /// </summary>
                public const string Period = "period";

                /// <summary>
                /// Bit mapped values from <see cref="StatusCode"/>.
                /// </summary>
                public const string ChartStatus = "chartstatus";

                /// <summary>
                /// The total number of log entries for this domain
                /// </summary>
                public const string LogEntryCount = "logentrycount";

                public class Calculated
                {
                    /// <summary>
                    /// Number of import files for this domain.
                    /// </summary>
                    public const string ImportFileCount = "CalcImportCount";
                }
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

                /// <summary>
                /// The default for <see cref="Defs.Columns.Period"/>.
                /// </summary>
                public const long DefaultPeriod = 30;

                /// <summary>
                /// The default value for <see cref="Defs.Columns.ChartStatus"/>.
                /// </summary>
                public const long DefaultChartStatus = StatusCode.Code200.BitValue + StatusCode.Code404.BitValue;

                /// <summary>
                /// Provides static values for <see cref="Defs.Columns.DisplayMode"/>
                /// </summary>
                public static class DisplayMode
                {
                    /// <summary>
                    /// Raw data display mode
                    /// </summary>
                    public const long Raw = 1;

                    /// <summary>
                    /// Chart display mode
                    /// </summary>
                    public const long Chart = 2;
                }
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
            return Create(Defs.Values.NewDomainDisplayName, Defs.Values.NewDomainPreface);
        }

        /// <summary>
        /// Creates a record with the specified name and preface.
        /// </summary>
        /// <param name="displayName">The display name</param>
        /// <param name="preface">The preface</param>
        /// <returns>The newly added <see cref="DomainRow"/></returns> 
        public DomainRow Create(string displayName, string preface)
        {
            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException(nameof(displayName));
            }

            if (string.IsNullOrEmpty(preface))
            {
                throw new ArgumentNullException(nameof(preface));
            }

            var obj = new DomainRow(NewRow())
            {
                DisplayName = displayName,
                Preface = preface,
                DisplayMode = Defs.Values.DisplayMode.Raw,
                Period = Defs.Values.DefaultPeriod,
                ChartStatus = Defs.Values.DefaultChartStatus,
                LogEntryCount = 0
            };
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

        /// <summary>
        /// Updates the log entry count for each domain.
        /// </summary>
        public void UpdateLogEntryCount()
        {
            foreach (DomainRow domain in EnumerateAll())
            {
                domain.LogEntryCount = GetLogEntryCount(domain);
            }
            Save();
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
                { Defs.Columns.DisplayMode, ColumnType.Integer, false, false, Defs.Values.DisplayMode.Raw },
                { Defs.Columns.Period, ColumnType.Integer, false, false, Defs.Values.DefaultPeriod },
                { Defs.Columns.ChartStatus, ColumnType.Integer, false, false, Defs.Values.DefaultChartStatus },
                { Defs.Columns.LogEntryCount, ColumnType.Integer, false, false, 0L }
            };
        }

        /// <inheritdoc/>
        protected override void SetDataRelations()
        {
            CreateParentChildRelation<ImportFileTable>(Defs.Relations.ToImportFile, Defs.Columns.Id, ImportFileTable.Defs.Columns.DomainId);
        }

        /// <inheritdoc/>
        protected override void UseDataRelations()
        {
            string expr = string.Format("Count(Child({0}).{1})", Defs.Relations.ToImportFile, ImportFileTable.Defs.Columns.Id);
            CreateExpressionColumn<long>(Defs.Columns.Calculated.ImportFileCount, expr);
        }

        /// <summary>
        /// Gets a list of column names to use in subsequent initial insert operations.
        /// These are used only when the table is empty, i.e. upon first creation.
        /// </summary>
        /// <returns>A list of column names</returns>
        protected override List<string> GetPopulateColumnList()
        {
            return new List<string>() { Defs.Columns.Id, Defs.Columns.DisplayName, Defs.Columns.Preface, Defs.Columns.Period };
        }

        /// <summary>
        /// Provides an enumerable that returns values for each row to be populated.
        /// </summary>
        /// <returns>An IEnumerable</returns>
        protected override IEnumerable<object[]> EnumeratePopulateValues()
        {
            yield return new object[] { Defs.Values.DomainZeroId, Defs.Values.DomainZeroDisplayName, Defs.Values.DomainZeroPreface, Defs.Values.DefaultPeriod };
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private long GetLogEntryCount(DomainRow domain)
        {
            string sql = $"select count(*) from {Namespace}.{LogEntryTable.Defs.TableName} where {LogEntryTable.Defs.Columns.DomainId}={domain.Id}";
            object result = Controller.Execution.Scalar(sql);
            return (long)result;
        }
        #endregion
    }
}
using Restless.Toolkit.Core.Database.SQLite;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Restless.Logite.Database.Tables
{
    public class IpAddressTable : Core.ApplicationTableBase
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
            public const string TableName = "ipaddress";

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
                public const string IpAddress = "ipaddress";

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
                /// The name of the relation that relates the <see cref="IpAddressTable"/> to the <see cref="LogEntryTable"/>.
                /// </summary>
                public const string ToLogEntry = "IpToLogEntry";
            }

            /// <summary>
            /// Provides static values.
            /// </summary>
            public static class Values
            {
                /// <summary>
                /// The id for the "No ip address" entry.
                /// </summary>
                public const long IpAddressZeroId = 0;

                /// <summary>
                /// The name for the "No ip address" entry.
                /// </summary>
                public const string IpAddressZeroName = "--";
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="IpAddressTable"/> class.
        /// </summary>
        public IpAddressTable() : base(Defs.TableName)
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
                { Defs.Columns.IpAddress, ColumnType.Text },
            };
        }

        /// <summary>
        /// Gets a list of column names to use in subsequent initial insert operations.
        /// These are used only when the table is empty, i.e. upon first creation.
        /// </summary>
        /// <returns>A list of column names</returns>
        protected override List<string> GetPopulateColumnList()
        {
            return new List<string>() { Defs.Columns.Id, Defs.Columns.IpAddress };
        }

        /// <summary>
        /// Provides an enumerable that returns values for each row to be populated.
        /// </summary>
        /// <returns>An IEnumerable</returns>
        protected override IEnumerable<object[]> EnumeratePopulateValues()
        {
            yield return new object[] { Defs.Values.IpAddressZeroId, Defs.Values.IpAddressZeroName };
        }

        /// <inheritdoc/>
        protected override void SetDataRelations()
        {
            CreateParentChildRelation<LogEntryTable>(Defs.Relations.ToLogEntry, Defs.Columns.Id, LogEntryTable.Defs.Columns.IpAddressId);
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
        /// Inserts an ip address record if it doesn't yet exist.
        /// </summary>
        /// <param name="ipAddress">The ip address</param>
        /// <returns>The newly inserted id, or the existing id</returns>
        internal long InsertIf(string ipAddress)
        {
            if (!string.IsNullOrEmpty(ipAddress))
            {
                DataRow row = GetUniqueRow(Select($"{Defs.Columns.IpAddress}='{ipAddress}'"));
                if (row != null)
                {
                    return (long)row[Defs.Columns.Id];
                }

                row = NewRow();
                row[Defs.Columns.IpAddress] = ipAddress;
                Rows.Add(row);
                Save();
                return (long)row[Defs.Columns.Id];
            }

            return Defs.Values.IpAddressZeroId;
        }
        #endregion

    }
}

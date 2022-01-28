using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System.Collections.Generic;
using System.Data;

namespace Restless.Logite.Database.Tables
{
    public class StatusTable : RawTable<StatusRow>
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
            public const string TableName = "status";

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
                /// The request status, 200, 404, etc.
                /// </summary>
                public const string Status = "status";
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusTable"/> class.
        /// </summary>
        public StatusTable() : base(Defs.TableName)
        {
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
                { Defs.Columns.Status, ColumnType.Integer },
            };
        }

        /// <summary>
        /// Gets a list of column names to use in subsequent initial insert operations.
        /// These are used only when the table is empty, i.e. upon first creation.
        /// </summary>
        /// <returns>A list of column names</returns>
        protected override List<string> GetPopulateColumnList()
        {
            return new List<string>() { Defs.Columns.Id, Defs.Columns.Status };
        }

        /// <summary>
        /// Provides an enumerable that returns values for each row to be populated.
        /// </summary>
        /// <returns>An IEnumerable</returns>
        protected override IEnumerable<object[]> EnumeratePopulateValues()
        {
            yield return new object[] { 1, 200 };
            yield return new object[] { 2, 302 };
            yield return new object[] { 3, 304 };
            yield return new object[] { 4, 400 };
            yield return new object[] { 5, 404 };
            yield return new object[] { 6, 444 };
            yield return new object[] { 7, 500 };
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Inserts a status record if it doesn't yet exist.
        /// </summary>
        /// <param name="entry">The entry</param>
        /// <returns>The newly inserted id, or the existing id</returns>
        internal long InsertIf(LogEntry entry)
        {
            DataRow row = GetUniqueRow(Select($"{Defs.Columns.Status}={entry.Status}"));
            if (row != null)
            {
                return (long)row[Defs.Columns.Id];
            }

            row = NewRow();
            row[Defs.Columns.Status] = entry.Status;
            Rows.Add(row);
            Save();
            return (long)row[Defs.Columns.Id];
        }

        internal override void Load(long domainId, IdCollection ids)
        {
            string sql =
                $"SELECT S.{Defs.Columns.Id},S.{Defs.Columns.Status}," +
                $"COUNT(L.{LogEntryTable.Defs.Columns.Id}) " +
                $"FROM {Defs.TableName} S " +
                $"LEFT JOIN {LogEntryTable.Defs.TableName} L " +
                $"ON (S.{Defs.Columns.Status}=L.{LogEntryTable.Defs.Columns.Status} AND L.{LogEntryTable.Defs.Columns.DomainId}={domainId}) " +
                $"WHERE S.{Defs.Columns.Status} IN ({ids})" +
                $"GROUP BY S.{Defs.Columns.Status}";

            LoadFromSql(sql, (reader) =>
            {
                return new StatusRow()
                {
                    Id = reader.GetInt64(0),
                    Status = reader.GetInt64(1),
                    UsageCount = reader.GetInt64(2)
                };
            });
        }
        #endregion
    }
}
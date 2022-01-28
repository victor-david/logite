using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Lookup table for http methods
    /// </summary>
    public class MethodTable : RawTable<MethodRow>
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
            public const string TableName = "method";

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
                /// The method name.
                /// </summary>
                public const string Method = "method";
            }

            /// <summary>
            /// Provides static values.
            /// </summary>
            public static class Values
            {
                /// <summary>
                /// The id for the "No method" entry.
                /// </summary>
                public const long MethodZeroId = 0;

                /// <summary>
                /// The name for the "No method" entry.
                /// </summary>
                public const string MethodZeroName = "--";
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodTable"/> class.
        /// </summary>
        public MethodTable() : base(Defs.TableName)
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
                { Defs.Columns.Method, ColumnType.Text },
            };
        }

        /// <summary>
        /// Gets a list of column names to use in subsequent initial insert operations.
        /// These are used only when the table is empty, i.e. upon first creation.
        /// </summary>
        /// <returns>A list of column names</returns>
        protected override List<string> GetPopulateColumnList()
        {
            return new List<string>() { Defs.Columns.Id, Defs.Columns.Method };
        }

        /// <summary>
        /// Provides an enumerable that returns values for each row to be populated.
        /// </summary>
        /// <returns>An IEnumerable</returns>
        protected override IEnumerable<object[]> EnumeratePopulateValues()
        {
            yield return new object[] { Defs.Values.MethodZeroId, Defs.Values.MethodZeroName };
            yield return new object[] { Defs.Values.MethodZeroId + 1, "GET" };
            yield return new object[] { Defs.Values.MethodZeroId + 2, "POST" };
            yield return new object[] { Defs.Values.MethodZeroId + 3, "HEAD" };
            yield return new object[] { Defs.Values.MethodZeroId + 4, "PUT" };
            yield return new object[] { Defs.Values.MethodZeroId + 5, "DELETE" };
            yield return new object[] { Defs.Values.MethodZeroId + 6, "CONNECT" };
            yield return new object[] { Defs.Values.MethodZeroId + 7, "OPTIONS" };
            yield return new object[] { Defs.Values.MethodZeroId + 8, "TRACE" };
            yield return new object[] { Defs.Values.MethodZeroId + 9, "PATCH" };
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Loads method data.
        /// </summary>
        /// <remarks>
        /// This method is called to have the methods loaded prior to an import operation.
        /// MethodTable does not use <see cref="RawTable{T}.InsertEntryIf(LogEntry)"/>
        /// because methods are inserted into the table at creation time; no new methods
        /// may be added.
        /// </remarks>
        internal void LoadMethodData()
        {
            ClearRaw();
            string sql =
                $"select {Defs.Columns.Id},{Defs.Columns.Method} " +
                $"from {Namespace}.{TableName} " +
                $"order by {Defs.Columns.Id}";

            LoadFromSql(sql, (reader) =>
            {
                return new MethodRow()
                {
                    Id = reader.GetInt64(0),
                    Method = reader.GetString(1)
                };
            });
        }

        /// <summary>
        /// Gets the method id for the specified method.
        /// </summary>
        /// <param name="entry">The log entry</param>
        /// <returns>The method id</returns>
        internal long GetMethodId(LogEntry entry)
        {
            if (!string.IsNullOrEmpty(entry.Method))
            {
                foreach (MethodRow row in EnumerateAll())
                {
                    if (row.Method.Equals(entry.Method, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return row.Id;
                    }
                }
            }
            return Defs.Values.MethodZeroId;
        }

        internal override void Load(long domainId, IdCollection ids)
        {
            string sql =
                $"SELECT M.{Defs.Columns.Id},{Defs.Columns.Method}," +
                $"COUNT(L.{LogEntryTable.Defs.Columns.Id}) " +
                $"FROM {Defs.TableName} M " +
                $"JOIN {LogEntryTable.Defs.TableName} L " +
                $"ON (M.{Defs.Columns.Id}=L.{LogEntryTable.Defs.Columns.MethodId} AND L.{LogEntryTable.Defs.Columns.DomainId}={domainId}) " +
                $"WHERE M.{Defs.Columns.Id} IN ({ids}) " +
                $"GROUP BY M.id";

            LoadFromSql(sql, (reader) =>
            {
                return new MethodRow()
                {
                    Id = reader.GetInt64(0),
                    Method = reader.GetString(1),
                    UsageCount = reader.GetInt64(2)
                };
            });
        }
        #endregion
    }
}
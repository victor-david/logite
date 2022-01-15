using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Lookup table for http methods
    /// </summary>
    public class MethodTable : Core.ApplicationTableBase
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

        #region Public methods
        /// <summary>
        /// Loads the data from the database into the Data collection for this table.
        /// </summary>
        public override void Load()
        {
            Load(null, Defs.Columns.Id);
        }

        /// <summary>
        /// Provides an enumerable that returns all records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MethodRow> EnumerateAll()
        {
            foreach (var row in EnumerateRows(null, Defs.Columns.Id))
            {
                yield return new MethodRow(row);
            }
        }

        /// <summary>
        /// Gets the method id for the specified method.
        /// </summary>
        /// <param name="method">The method string</param>
        /// <returns>The method id</returns>
        public long GetMethodId(string method)
        {
            if (!string.IsNullOrEmpty(method))
            {
                foreach (MethodRow row in EnumerateAll())
                {
                    if (row.Method.Equals(method, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return row.Id;
                    }
                }
            }
            return Defs.Values.MethodZeroId;
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

        #endregion

    }
}

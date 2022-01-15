using Restless.Toolkit.Core.Database.SQLite;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Restless.Logite.Database.Tables
{
    public class RequestTable : Core.ApplicationTableBase
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
            }

            /// <summary>
            /// Provides static values.
            /// </summary>
            public static class Values
            {
                /// <summary>
                /// The id for the "No request" entry.
                /// </summary>
                public const long RequestZeroId = 0;

                /// <summary>
                /// The name for the "No request" entry.
                /// </summary>
                public const string RequestZeroName = "--";

                /// <summary>
                /// When a request is a byte attack.
                /// </summary>
                public const string ByteAttackRequest = "Byte Attack";
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
                { Defs.Columns.Request, ColumnType.Text },
            };
        }

        /// <summary>
        /// Gets a list of column names to use in subsequent initial insert operations.
        /// These are used only when the table is empty, i.e. upon first creation.
        /// </summary>
        /// <returns>A list of column names</returns>
        protected override List<string> GetPopulateColumnList()
        {
            return new List<string>() { Defs.Columns.Id, Defs.Columns.Request };
        }

        /// <summary>
        /// Provides an enumerable that returns values for each row to be populated.
        /// </summary>
        /// <returns>An IEnumerable</returns>
        protected override IEnumerable<object[]> EnumeratePopulateValues()
        {
            yield return new object[] { Defs.Values.RequestZeroId, Defs.Values.RequestZeroName };
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Inserts a request record if it doesn't yet exist.
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>The newly inserted id, or the existing id</returns>
        internal long InsertIf(string request)
        {
            if (!string.IsNullOrEmpty(request))
            {
                DataRow row = GetUniqueRow(Select($"{Defs.Columns.Request}='{request}'"));
                if (row != null)
                {
                    return (long)row[Defs.Columns.Id];
                }

                row = NewRow();
                row[Defs.Columns.Request] = request;
                Rows.Add(row);
                Save();
                return (long)row[Defs.Columns.Id];
            }

            return Defs.Values.RequestZeroId;
        }
        #endregion

    }
}

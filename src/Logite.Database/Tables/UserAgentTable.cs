using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    public class UserAgentTable : Core.ApplicationTableBase
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
            public const string TableName = "useragent";

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
                /// The user agent string
                /// </summary>
                public const string Agent = "agent";
            }

            /// <summary>
            /// Provides static values.
            /// </summary>
            public static class Values
            {
                /// <summary>
                /// The id for the "No user agent" entry.
                /// </summary>
                public const long UserAgentZeroId = 0;

                /// <summary>
                /// The name for the "No user agent" entry.
                /// </summary>
                public const string UserAgentZeroName = "--";

                /// <summary>
                /// The name for an attack user agent.
                /// </summary>
                public const string UserAgentAttackName = "User Agent attack";
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentTable"/> class.
        /// </summary>
        public UserAgentTable() : base(Defs.TableName)
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
                { Defs.Columns.Agent, ColumnType.Text}
            };
        }

        /// <summary>
        /// Gets a list of column names to use in subsequent initial insert operations.
        /// These are used only when the table is empty, i.e. upon first creation.
        /// </summary>
        /// <returns>A list of column names</returns>
        protected override List<string> GetPopulateColumnList()
        {
            return new List<string>() { Defs.Columns.Id, Defs.Columns.Agent };
        }

        /// <summary>
        /// Provides an enumerable that returns values for each row to be populated.
        /// </summary>
        /// <returns>An IEnumerable</returns>
        protected override IEnumerable<object[]> EnumeratePopulateValues()
        {
            yield return new object[] { Defs.Values.UserAgentZeroId, Defs.Values.UserAgentZeroName };
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Inserts a user agent record if it doesn't yet exist.
        /// </summary>
        /// <param name="agent">The agent</param>
        /// <returns>The newly inserted id, or the existing id</returns>
        internal long InsertIf(string agent)
        {
            if (!string.IsNullOrEmpty(agent))
            {
                DataRow row = GetUniqueRow(Select($"{Defs.Columns.Agent}='{agent}'"));
                if (row != null)
                {
                    return (long)row[Defs.Columns.Id];
                }

                row = NewRow();
                row[Defs.Columns.Agent] = agent;
                Rows.Add(row);
                Save();
                return (long)row[Defs.Columns.Id];
            }
            return Defs.Values.UserAgentZeroId;
        }
        #endregion
    }
}

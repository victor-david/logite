using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    public class LogEntryTable : Core.ApplicationTableBase
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
            public const string TableName = "logentry";

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
                /// The remote address for the log entry.
                /// </summary>
                public const string RemoteAddress = "remoteaddress";

                /// <summary>
                /// The remote user.
                /// </summary>
                public const string RemoteUser = "remoteuser";

                /// <summary>
                /// Timestamp of the log entry.
                /// </summary>
                public const string Timestamp = "timestamp";

                /// <summary>
                /// The request status, 200, 404, etc.
                /// </summary>
                public const string Status = "status";

                /// <summary>
                /// The number of bytes sent.
                /// </summary>
                public const string BytesSent = "bytes";

                /// <summary>
                /// Id of the request
                /// </summary>
                public const string RequestId = "requestid";

                /// <summary>
                /// Id of the referer
                /// </summary>
                public const string RefererId = "refererid";

                /// <summary>
                /// Id of the user agent
                /// </summary>
                public const string UserAgentId = "agentid";

            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntryTable"/> class.
        /// </summary>
        public LogEntryTable() : base(Defs.TableName)
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
                { Defs.Columns.RemoteAddress, ColumnType.Text, false, false, null, IndexType.Index},
                { Defs.Columns.RemoteUser, ColumnType.Text, false, true },
                { Defs.Columns.Timestamp, ColumnType.Timestamp, false, false, null, IndexType.Index },
                { Defs.Columns.Status, ColumnType.Integer, false, false, 0L, IndexType.Index },
                { Defs.Columns.BytesSent, ColumnType.Integer, false, false },
                { Defs.Columns.RequestId, ColumnType.Integer, false, false, 0L, IndexType.Index },
                { Defs.Columns.RefererId, ColumnType.Integer, false, false, 0L, IndexType.Index },
                { Defs.Columns.UserAgentId, ColumnType.Integer, false, false, UserAgentTable.Defs.Values.UserAgentZeroId, IndexType.Index }
            };
        }
        #endregion


    }
}

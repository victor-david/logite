using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System.Data;

namespace Restless.Logite.Database.Tables
{
    public class LogEntryTable : DemandDomainTable
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
                public const string Id = IdColumnName;

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
                /// Length of attack byte string, or zero
                /// </summary>
                public const string AttackLength = "attacklength";

                /// <summary>
                /// The http version, i.e 1.0, 1.1, or 0.0
                /// </summary>
                public const string HttpVersion = "http";

                /// <summary>
                /// The id of the import file.
                /// </summary>
                public const string ImportFileId = "importid";

                /// <summary>
                /// The line number of the import file.
                /// </summary>
                public const string ImportLineNumber = "importline";

                /// <summary>
                /// Id of the domain
                /// </summary>
                public const string DomainId = DomainIdColumnName;

                /// <summary>
                /// Id of the ip address entry
                /// </summary>
                public const string IpAddressId = "ipaddressid";

                /// <summary>
                /// Id of the method
                /// </summary>
                public const string MethodId = "methodid";

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

                /// <summary>
                /// Provides static column names for columns that are calculated from other values.
                /// </summary>
                public class Calculated
                {
                    /// <summary>
                    /// Ip Address
                    /// </summary>
                    public const string IpAddress = "CalcIpAddress";

                    /// <summary>
                    /// Method
                    /// </summary>
                    public const string Method = "CalcMethod";

                    /// <summary>
                    /// Request
                    /// </summary>
                    public const string Request = "CalcRequest";

                    /// <summary>
                    /// Referer
                    /// </summary>
                    public const string Referer = "CalcReferer";
                }

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
                { Defs.Columns.RemoteUser, ColumnType.Text, false, true },
                { Defs.Columns.Timestamp, ColumnType.Timestamp, false, false, null, IndexType.Index },
                { Defs.Columns.Status, ColumnType.Integer, false, false, 0L, IndexType.Index },
                { Defs.Columns.BytesSent, ColumnType.Integer, false, false, 0L },
                { Defs.Columns.AttackLength, ColumnType.Integer, false, false, 0L },
                { Defs.Columns.HttpVersion, ColumnType.Text },
                { Defs.Columns.ImportFileId, ColumnType.Integer, false, false, 0L, IndexType.Index },
                { Defs.Columns.ImportLineNumber, ColumnType.Integer },
                { Defs.Columns.DomainId, ColumnType.Integer, false, false, DomainTable.Defs.Values.DomainZeroId, IndexType.Index },
                { Defs.Columns.IpAddressId, ColumnType.Integer, false, false, 0L, IndexType.Index },
                { Defs.Columns.MethodId, ColumnType.Integer, false, false, MethodTable.Defs.Values.MethodZeroId, IndexType.Index },
                { Defs.Columns.RequestId, ColumnType.Integer, false, false, null, IndexType.Index },
                { Defs.Columns.RefererId, ColumnType.Integer, false, false, null, IndexType.Index },
                { Defs.Columns.UserAgentId, ColumnType.Integer, false, false, null, IndexType.Index }
            };
        }

        protected override void UseDataRelations()
        {
            CreateChildToParentColumn(Defs.Columns.Calculated.IpAddress, IpAddressTable.Defs.Relations.ToLogEntry, IpAddressTable.Defs.Columns.IpAddress);
            CreateChildToParentColumn(Defs.Columns.Calculated.Method, MethodTable.Defs.Relations.ToLogEntry, MethodTable.Defs.Columns.Method);
            CreateChildToParentColumn(Defs.Columns.Calculated.Request, RequestTable.Defs.Relations.ToLogEntry, RequestTable.Defs.Columns.Request);
            CreateChildToParentColumn(Defs.Columns.Calculated.Referer, RefererTable.Defs.Relations.ToLogEntry, RefererTable.Defs.Columns.Referer);
        }

        protected override string GetAdditonalLoadWhere(DomainRow domain)
        {
            return $"{Defs.Columns.Timestamp} > date('now','-2 day')";
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        internal void Insert(LogEntry entry, long ipAddressId, long methodId, long requestId, long refererId, long agentId)
        {
            DataRow row = NewRow();
            row[Defs.Columns.RemoteUser] = entry.RemoteUser;
            row[Defs.Columns.Timestamp] = entry.RequestTime;
            row[Defs.Columns.Status] = entry.Status;
            row[Defs.Columns.BytesSent] = entry.BytesSent;
            row[Defs.Columns.AttackLength] = entry.AttackLength;
            row[Defs.Columns.HttpVersion] = entry.HttpVersion;
            row[Defs.Columns.ImportFileId] = entry.ImportFileId;
            row[Defs.Columns.ImportLineNumber] = entry.ImportFileLineNumber;
            row[Defs.Columns.DomainId] = entry.DomainId;
            row[Defs.Columns.IpAddressId] = ipAddressId;
            row[Defs.Columns.MethodId] = methodId;
            row[Defs.Columns.RequestId] = requestId;
            row[Defs.Columns.RefererId] = refererId;
            row[Defs.Columns.UserAgentId] = agentId;
            Rows.Add(row);
            Save();
        }
        #endregion
    }
}

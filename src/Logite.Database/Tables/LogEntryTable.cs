﻿using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System.Data;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    public class LogEntryTable : DemandDomainTable
    {
        private IdCollection ipId;
        private IdCollection requestId;
        private IdCollection refererId;
        private IdCollection agentId;

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
                public const string DomainId = "domainid";

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
                /// The id of the request attack, or zero
                /// </summary>
                public const string AttackIdRequest = "attackidrequest";

                /// <summary>
                /// The id of the referer attack, or zero
                /// </summary>
                public const string AttackIdReferer = "attackidreferer";

                /// <summary>
                /// The id of the user agent attack, or zero
                /// </summary>
                public const string AttackIdAgent = "attackidagent";

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
            ipId = new IdCollection();
            requestId = new IdCollection();
            refererId = new IdCollection();
            agentId = new IdCollection();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        public void LoadDomain(DomainRow domain)
        {
            DataSet.EnforceConstraints = false;
            LoadDomainPrivate(domain);
            DataSet.EnforceConstraints = true;
        }

        public void UnloadDomain()
        {
            DataSet.EnforceConstraints = false;
            UnloadDomainPrivate();
            DataSet.EnforceConstraints = true;
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
                { Defs.Columns.RemoteUser, ColumnType.Text, false, true },
                { Defs.Columns.Timestamp, ColumnType.Timestamp, false, false, null, IndexType.Index },
                { Defs.Columns.Status, ColumnType.Integer, false, false, 0L, IndexType.Index },
                { Defs.Columns.BytesSent, ColumnType.Integer, false, false, 0L },
                { Defs.Columns.HttpVersion, ColumnType.Text },
                { Defs.Columns.ImportFileId, ColumnType.Integer, false, false, 0L, IndexType.Index },
                { Defs.Columns.ImportLineNumber, ColumnType.Integer },
                { Defs.Columns.DomainId, ColumnType.Integer, false, false, DomainTable.Defs.Values.DomainZeroId, IndexType.Index },
                { Defs.Columns.IpAddressId, ColumnType.Integer, false, false, 0L, IndexType.Index },
                { Defs.Columns.MethodId, ColumnType.Integer, false, false, MethodTable.Defs.Values.MethodZeroId, IndexType.Index },
                { Defs.Columns.RequestId, ColumnType.Integer, false, false, null, IndexType.Index },
                { Defs.Columns.RefererId, ColumnType.Integer, false, false, null, IndexType.Index },
                { Defs.Columns.UserAgentId, ColumnType.Integer, false, false, null, IndexType.Index },
                { Defs.Columns.AttackIdRequest, ColumnType.Integer, false, false, AttackTable.Defs.Values.AttackZeroId, IndexType.Index },
                { Defs.Columns.AttackIdReferer, ColumnType.Integer, false, false, AttackTable.Defs.Values.AttackZeroId, IndexType.Index },
                { Defs.Columns.AttackIdAgent, ColumnType.Integer, false, false, AttackTable.Defs.Values.AttackZeroId, IndexType.Index }

            };
        }

        protected override void UseDataRelations()
        {
            CreateChildToParentColumn(Defs.Columns.Calculated.IpAddress, IpAddressTable.Defs.Relations.ToLogEntry, IpAddressTable.Defs.Columns.IpAddress);
            CreateChildToParentColumn(Defs.Columns.Calculated.Method, MethodTable.Defs.Relations.ToLogEntry, MethodTable.Defs.Columns.Method);
            CreateChildToParentColumn(Defs.Columns.Calculated.Request, RequestTable.Defs.Relations.ToLogEntry, RequestTable.Defs.Columns.Request);
            CreateChildToParentColumn(Defs.Columns.Calculated.Referer, RefererTable.Defs.Relations.ToLogEntry, RefererTable.Defs.Columns.Referer);
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        internal void Insert(LogEntry entry, LogEntryIds ids)
        {
            StringBuilder sql = new StringBuilder($"insert into {Namespace}.{TableName} (", 512);
            if (!string.IsNullOrEmpty(entry.RemoteUser))
            {
                sql.Append($"{Defs.Columns.RemoteUser},");
            }
            sql.Append($"{Defs.Columns.Timestamp},");
            sql.Append($"{Defs.Columns.Status},");
            sql.Append($"{Defs.Columns.BytesSent},");
            sql.Append($"{Defs.Columns.HttpVersion},");
            sql.Append($"{Defs.Columns.ImportFileId},");
            sql.Append($"{Defs.Columns.ImportLineNumber},");
            sql.Append($"{Defs.Columns.DomainId},");
            sql.Append($"{Defs.Columns.IpAddressId},");
            sql.Append($"{Defs.Columns.MethodId},");
            sql.Append($"{Defs.Columns.RequestId},");
            sql.Append($"{Defs.Columns.RefererId},");
            sql.Append($"{Defs.Columns.UserAgentId},");
            sql.Append($"{Defs.Columns.AttackIdRequest}, ");
            sql.Append($"{Defs.Columns.AttackIdReferer}, ");
            sql.Append($"{Defs.Columns.AttackIdAgent}) ");

            sql.Append("values (");
            if (!string.IsNullOrEmpty(entry.RemoteUser))
            {
                sql.Append($"'{entry.RemoteUser}'");
            }
            // 2022-01-20 01:16:52
            sql.Append($"'{entry.RequestTime:yyyy-MM-dd hh:mm:ss}',");
            sql.Append($"{entry.Status},");
            sql.Append($"{entry.BytesSent},");
            sql.Append($"'{entry.HttpVersion}',");
            sql.Append($"{entry.ImportFileId},");
            sql.Append($"{entry.ImportFileLineNumber},");
            sql.Append($"{entry.DomainId},");
            sql.Append($"{ids.IpAddressId},");
            sql.Append($"{ids.MethodId},");
            sql.Append($"{ids.RequestId},");
            sql.Append($"{ids.RefererId},");
            sql.Append($"{ids.AgentId},");
            sql.Append($"{ids.AttackRequestId},");
            sql.Append($"{ids.AttackRefererId},");
            sql.Append($"{ids.AttackAgentId})");


            Controller.Execution.NonQuery(sql.ToString());
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void LoadDomainPrivate(DomainRow domain)
        {
            Clear();
            ClearIdCollections();

            string sql = $"SELECT * FROM {Namespace}.{TableName} WHERE {Defs.Columns.DomainId}={domain.Id} AND  {Defs.Columns.Timestamp} > date('now','-{domain.PastDays} day')";
            Load(Controller.Execution.Query(sql));

            foreach (DataRow row in Rows)
            {
                ipId.Add((long)row[Defs.Columns.IpAddressId]);
                requestId.Add((long)row[Defs.Columns.RequestId]);
                refererId.Add((long)row[Defs.Columns.RefererId]);
                agentId.Add((long)row[Defs.Columns.UserAgentId]);
            }
            Controller.GetTable<IpAddressTable>().Load(ipId);
            Controller.GetTable<RequestTable>().Load(requestId);
            Controller.GetTable<RefererTable>().Load(refererId);
            Controller.GetTable<UserAgentTable>().Load(agentId);
        }

        private void UnloadDomainPrivate()
        {
            Clear();
            ClearIdCollections();
            Controller.GetTable<IpAddressTable>().Clear();
            Controller.GetTable<RequestTable>().Clear();
            Controller.GetTable<RefererTable>().Clear();
            Controller.GetTable<UserAgentTable>().Clear();
        }

        private void ClearIdCollections()
        {
            ipId.Clear();
            requestId.Clear();
            refererId.Clear();
            agentId.Clear();
        }
        #endregion
    }
}

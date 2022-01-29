using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    public class LogEntryTable : RawTable<LogEntryRow>
    {
        private IdCollection ipId;
        private IdCollection methodId;
        private IdCollection statusId;

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
            methodId = new IdCollection();
            statusId = new IdCollection();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        public void LoadDomain(DomainRow domain)
        {
            DataSet.EnforceConstraints = false;
            LoadRawDomainData(domain);
            DataSet.EnforceConstraints = true;
        }

        public void UnloadDomain()
        {
            DataSet.EnforceConstraints = false;
            UnloadRawDomainData();
            DataSet.EnforceConstraints = true;
        }

        public DataPointCollection<CountDataPoint> GetTotalTrafficData(DomainRow domain)
        {
            return GetDateCountCollection<CountDataPoint>(domain, (points, logEntryRow) => 
            {
                points.Add(CountDataPoint.Create(logEntryRow.Timestamp)).Count++;
            });
        }

        public DataPointCollection<StatusDataPoint> GetStatusTrafficData(DomainRow domain)
        {
            return GetDateCountCollection<StatusDataPoint>(domain, (points, logEntryRow) =>
            {
                points.Add(StatusDataPoint.Create(logEntryRow.Timestamp)).IncrementStatusCount(logEntryRow.Status);
            });
        }

        public DataPointCollection<CountDataPoint> GetUniqueIpTrafficData(DomainRow domain)
        {
            Dictionary<DateTime, List<long>> ips = new Dictionary<DateTime, List<long>>();

            return GetDateCountCollection<CountDataPoint>(domain, (points, logEntryRow) =>
            {
                CountDataPoint point = points.Add(CountDataPoint.Create(logEntryRow.Timestamp));
                if (!ips.ContainsKey(point.Date))
                {
                    ips.Add(point.Date, new List<long>());
                }

                if (!ips[point.Date].Contains(logEntryRow.IpAddressId))
                {
                    ips[point.Date].Add(logEntryRow.IpAddressId);
                    point.Count++;
                }
            });
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        private void LoadRawDomainData(DomainRow domain)
        {
            UnloadRawDomainData();

            HashSet<string> ignored = domain.GetIgnoredSet();

            string sql =
                $"SELECT " +
                $"L.{Defs.Columns.Id},{Defs.Columns.RemoteUser},{Defs.Columns.Timestamp},{Defs.Columns.Status}," +
                $"{Defs.Columns.BytesSent},{Defs.Columns.HttpVersion},{Defs.Columns.DomainId},{Defs.Columns.IpAddressId}," +
                $"{Defs.Columns.MethodId},{Defs.Columns.RequestId},{Defs.Columns.RefererId},{Defs.Columns.UserAgentId}," +
                $"{Defs.Columns.AttackIdRequest},{Defs.Columns.AttackIdReferer},{Defs.Columns.AttackIdAgent}," +
                $"IP.{IpAddressTable.Defs.Columns.IpAddress}," +
                $"M.{MethodTable.Defs.Columns.Method}," +
                $"R.{RequestTable.Defs.Columns.Request}," +
                $"RF.{RefererTable.Defs.Columns.Referer} " +
                $"FROM {Namespace}.{TableName} L " +
                $"LEFT JOIN {IpAddressTable.Defs.TableName} IP ON (L.{Defs.Columns.IpAddressId} = IP.{IpAddressTable.Defs.Columns.Id}) " +
                $"LEFT JOIN {MethodTable.Defs.TableName} M ON (L.{Defs.Columns.MethodId} = M.{MethodTable.Defs.Columns.Id}) " +
                $"LEFT JOIN {RequestTable.Defs.TableName} R ON (L.{Defs.Columns.RequestId} = R.{RequestTable.Defs.Columns.Id}) " +
                $"LEFT JOIN {RefererTable.Defs.TableName} RF ON (L.{Defs.Columns.RefererId} = RF.{RefererTable.Defs.Columns.Id}) " +
                $"WHERE {Defs.Columns.DomainId}={domain.Id} AND {Defs.Columns.Timestamp} > date('now','-{domain.Period} day')";

            LoadFromSql(sql, (reader) =>
            {
                string request = reader.GetString(17);

                if (IsRequestIncluded(ignored, request))
                {
                    return new LogEntryRow()
                    {
                        Id = reader.GetInt64(0),
                        RemoteUser = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Timestamp = reader.GetDateTime(2),
                        Status = reader.GetInt64(3),
                        BytesSent = reader.GetInt64(4),
                        HttpVersion = reader.GetString(5),
                        DomainId = reader.GetInt64(6),
                        IpAddressId = reader.GetInt64(7),
                        MethodId = reader.GetInt64(8),
                        RequestId = reader.GetInt64(9),
                        RefererId = reader.GetInt64(10),
                        AgentId = reader.GetInt64(11),
                        RequestAttackId = reader.GetInt64(12),
                        RefererAttackId = reader.GetInt64(13),
                        AgentAttackId = reader.GetInt64(14),
                        IpAddress = reader.GetString(15),
                        Method = reader.GetString(16),
                        Request = request,
                        Referer = reader.GetString(18)
                    };
                }
                return null;
            });

            foreach (LogEntryRow row in RawRows)
            {
                ipId.Add(row.IpAddressId);
                methodId.Add(row.MethodId);
                statusId.Add(row.Status);
            }

            Controller.GetTable<IpAddressTable>().Load(domain.Id, ipId);
            Controller.GetTable<MethodTable>().Load(domain.Id, methodId);
            Controller.GetTable<StatusTable>().Load(domain.Id, statusId);
        }

        private void UnloadRawDomainData()
        {
            Clear();
            ClearRaw();
            ipId.Clear();
            methodId.Clear();
            statusId.Clear();
            Controller.GetTable<IpAddressTable>().ClearRaw();
            Controller.GetTable<MethodTable>().ClearRaw();
            Controller.GetTable<RequestTable>().ClearRaw();
            Controller.GetTable<RefererTable>().ClearRaw();
            Controller.GetTable<UserAgentTable>().ClearRaw();
        }

        private DataPointCollection<T> GetDateCountCollection<T>(DomainRow domain, Action<DataPointCollection<T>, LogEntryRow> processor) where T : DataPoint
        {
            DataPointCollection<T> dataPoints = new DataPointCollection<T>();

            string sql =
                $"SELECT " +
                $"{Defs.Columns.Id},{Defs.Columns.Timestamp},{Defs.Columns.Status},{Defs.Columns.BytesSent}," +
                $"{Defs.Columns.IpAddressId},{Defs.Columns.MethodId},{Defs.Columns.RequestId},{Defs.Columns.RefererId},{Defs.Columns.UserAgentId}," +
                $"{Defs.Columns.AttackIdRequest},{Defs.Columns.AttackIdReferer},{Defs.Columns.AttackIdAgent} " +
                $"FROM {Namespace}.{TableName} " +
                $"WHERE {Defs.Columns.DomainId}={domain.Id} AND  {Defs.Columns.Timestamp} > date('now','-{domain.Period} day') " +
                $"ORDER BY {Defs.Columns.Timestamp}";

            using (IDataReader reader = Controller.Execution.Query(sql))
            {
                while (reader.Read())
                {
                    LogEntryRow row = GetLogEntryRow(reader);
                    processor(dataPoints, row);
                }
            }

            return dataPoints;
        }

        private LogEntryRow GetLogEntryRow(IDataReader reader)
        {
            return new LogEntryRow()
            {
                Id = reader.GetInt64(0),
                Timestamp = reader.GetDateTime(1),
                Status = reader.GetInt64(2),
                BytesSent = reader.GetInt64(3),
                IpAddressId = reader.GetInt64(4),
                MethodId = reader.GetInt64(5),
                RequestId = reader.GetInt64(6),
                RefererId = reader.GetInt64(7),
                AgentId = reader.GetInt64(8),
                RequestAttackId = reader.GetInt64(9),
                RefererAttackId = reader.GetInt64(10),
                AgentAttackId = reader.GetInt64(11)
            };
        }

        private bool IsRequestIncluded(HashSet<string> ignored, string request)
        {
            if (!string.IsNullOrEmpty(request))
            {
                foreach (string ignore in ignored)
                {
                    if (request.StartsWith(ignore, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
    }
}
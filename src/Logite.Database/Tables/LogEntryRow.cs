using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Columns = Restless.Logite.Database.Tables.LogEntryTable.Defs.Columns;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Encapsulates a single row from the <see cref="LogEntryTable"/>.
    /// </summary>
    /// <remarks>
    /// Unlike other classes of a similar type for other tables, the properties
    /// of this class are all read only. It's used when displaying log entry detail;
    /// the constructor loads attack data if it exists.
    /// </remarks>
    public class LogEntryRow : RawRow
    {
        #region Helper class
        public class Property
        {
            public string Name { get; }
            public string Value { get; }
            internal Property(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the id for this row object.
        /// </summary>
        public long Id
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the remote user
        /// </summary>
        public string RemoteUser
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the timestamp
        /// </summary>
        public DateTime Timestamp
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the status
        /// </summary>
        public long Status
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the bytes sent
        /// </summary>
        public long BytesSent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the http version
        /// </summary>
        public string HttpVersion
        {
            get;
            internal set;
        }

        public long DomainId
        {
            get;
            internal set;
        }

        public long IpAddressId
        {
            get;
            internal set;
        }

        public long MethodId
        {
            get;
            internal set;
        }

        public long RequestId
        {
            get;
            internal set;
        }

        public long RefererId
        {
            get;
            internal set;
        }

        public long AgentId
        {
            get;
            internal set;
        }

        public long AttackIdRequest
        {
            get;
            internal set;
        }

        public long AttackIdReferer
        {
            get;
            internal set;
        }

        public long AttackIdAgent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the ip address.
        /// </summary>
        public string IpAddress
        {
            get;
            internal set;
        }

        public string Method
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        public string Request
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the referer.
        /// </summary>
        public string Referer
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the request attack if any.
        /// </summary>
        public string RequestAttack
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the referer attack if any.
        /// </summary>
        public string RefererAttack
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the agent attack if any
        /// </summary>
        public string AgentAttack
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the enumerator that provides the properties.
        /// </summary>
        public IEnumerable<Property> PropertyEnumerator
        {
            get => EnumerateProperties();
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntryRow"/> class.
        /// </summary>
        internal LogEntryRow() : base (LogEntryTable.Defs.TableName)
        {
            RequestAttack = GetAttack(GetInt64(Columns.AttackIdRequest));
            RefererAttack = GetAttack(GetInt64(Columns.AttackIdReferer));
            AgentAttack = GetAttack(GetInt64(Columns.AttackIdAgent));
        }
        #endregion

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// Enumerates the properties. Exposed via <see cref="PropertyEnumerator"/> above.
        /// </summary>
        private IEnumerable<Property> EnumerateProperties()
        {
            yield return new Property(nameof(Timestamp), Timestamp.ToString());
            yield return new Property(nameof(IpAddress), IpAddress);
            yield return new Property(nameof(HttpVersion), HttpVersion);
            yield return new Property(nameof(Status), Status.ToString());
            yield return new Property(nameof(Request), Request);
            yield return new Property(nameof(Referer), Referer);
            yield return new Property(nameof(BytesSent), BytesSent.ToString());
            yield return new Property(nameof(RequestAttack), RequestAttack);
            yield return new Property(nameof(RefererAttack), RefererAttack);
            yield return new Property(nameof(AgentAttack), AgentAttack);
        }

        private string GetAttack(long attackId)
        {
            if (attackId == 0)
            {
                return "(none)";
            }

            string sql = $"select {AttackTable.Defs.Columns.AttackString} from {DatabaseController.MainAppSchemaName}.{AttackTable.Defs.TableName} where {AttackTable.Defs.Columns.Id}={attackId}";
            object result = DatabaseController.Instance.Execution.Scalar(sql);
            return result != null ? result.ToString() : "(failed)";
        }
        #endregion
    }
}
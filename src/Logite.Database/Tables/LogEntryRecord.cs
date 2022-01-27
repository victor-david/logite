using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Represents a log entry record obtained through direct data reader
    /// </summary>
    internal class LogEntryRecord
    {
        public long Id { get; internal set; }
        public DateTime Timestamp { get; internal set; }
        public long Status { get; internal set; }
        public long BytesSent { get; internal set; }
        public long IpAddressId { get; internal set; }
        public long MethodId { get; internal set; }
        public long RequestId { get; internal set; }
        public long RefererId { get; internal set; }
        public long UserAgentId { get; internal set; }
        public long AttackIdRequest { get; internal set; }
        public long AttackIdReferer { get; internal set; }
        public long AttackIdAgent { get; internal set; }

        internal LogEntryRecord()
        {
        }
    }
}
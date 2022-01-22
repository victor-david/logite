using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Ids used for a log entry
    /// </summary>
    internal struct LogEntryIds
    {
        internal long IpAddressId;
        internal long MethodId;
        internal long RequestId;
        internal long RefererId;
        internal long AgentId;
        internal long AttackRequestId;
        internal long AttackRefererId;
        internal long AttackAgentId;
    }
}

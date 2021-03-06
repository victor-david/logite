using Restless.Logite.Database.Tables;
using System.Diagnostics;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Log entry processor
    /// </summary>
    public static class LogEntryProcessor
    {
        private static LogEntryTable logEntryTable = null;
        private static IpAddressTable ipAddressTable = null;
        private static MethodTable methodTable = null;
        private static StatusTable statusTable = null;
        private static RefererTable refererTable = null;
        private static RequestTable requestTable = null;
        private static UserAgentTable agentTable = null;
        private static AttackTable attackTable = null;
        private static bool isInitialized = false;

        /// <summary>
        /// Initializes the log entry processor.
        /// </summary>
        public static void Init()
        {
            logEntryTable = DatabaseController.Instance.GetTable<LogEntryTable>();
            ipAddressTable = DatabaseController.Instance.GetTable<IpAddressTable>();
            methodTable = DatabaseController.Instance.GetTable<MethodTable>();
            statusTable = DatabaseController.Instance.GetTable<StatusTable>();
            refererTable = DatabaseController.Instance.GetTable<RefererTable>();
            requestTable = DatabaseController.Instance.GetTable<RequestTable>();
            agentTable = DatabaseController.Instance.GetTable<UserAgentTable>();
            attackTable = DatabaseController.Instance.GetTable<AttackTable>();
            methodTable.LoadMethodData();
            isInitialized = true;
        }

        /// <summary>
        /// Processes a single <see cref="LogEntry"/> object
        /// </summary>
        /// <param name="entry">The entry to process</param>
        public static void Process(LogEntry entry)
        {
            if (isInitialized)
            {
                LogEntryIds ids = new LogEntryIds()
                {
                    IpAddressId = ipAddressTable.InsertEntryIf(entry),
                    MethodId = methodTable.GetMethodId(entry),
                    RequestId = requestTable.InsertEntryIf(entry),
                    RefererId = refererTable.InsertEntryIf(entry),
                    AgentId = agentTable.InsertEntryIf(entry),
                    AttackRequestId = attackTable.Insert(entry, AttackVectorType.Request),
                    AttackRefererId = attackTable.Insert(entry, AttackVectorType.Referer),
                    AttackAgentId = attackTable.Insert(entry, AttackVectorType.UserAgent),
                };
                _ =  statusTable.InsertIf(entry);
                logEntryTable.Insert(entry, ids);
            }
        }
    }
}

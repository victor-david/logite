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
        private static RefererTable refererTable = null;
        private static RequestTable requestTable = null;
        private static UserAgentTable agentTable = null;
        private static bool isInitialized = false;

        /// <summary>
        /// Initializes the log entry processor.
        /// </summary>
        public static void Init()
        {
            logEntryTable = DatabaseController.Instance.GetTable<LogEntryTable>();
            ipAddressTable = DatabaseController.Instance.GetTable<IpAddressTable>();
            methodTable = DatabaseController.Instance.GetTable<MethodTable>();
            refererTable = DatabaseController.Instance.GetTable<RefererTable>();
            requestTable = DatabaseController.Instance.GetTable<RequestTable>();
            agentTable = DatabaseController.Instance.GetTable<UserAgentTable>();
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
                long ipAddressId = ipAddressTable.InsertIf(entry.RemoteAddress);
                long methodId = methodTable.GetMethodId(entry.Method);
                long requestId = requestTable.InsertIf(entry.Request);
                long refererId = refererTable.InsertIf(entry.Referer);
                long agentId = agentTable.InsertIf(entry.UserAgent);
                logEntryTable.Insert(entry, ipAddressId, methodId, requestId, refererId, agentId);
            }
        }
    }
}

using Restless.Logite.Database.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Core
{
    public class DemandDomainController
    {
        #region Private
        private List<DemandDomainTable> tables;
        #endregion

        /************************************************************************/

        #region Singleton access and constructor
        /// <summary>
        /// Gets the singleton instance of this class
        /// </summary>
        public static  DemandDomainController Instance { get; } = new DemandDomainController();

        /// <summary>
        /// Constructor (private)
        /// </summary>
        private DemandDomainController()
        {
            tables = new List<DemandDomainTable>()
            {
                DatabaseController.Instance.GetTable<IpAddressTable>(),
                DatabaseController.Instance.GetTable<RequestTable>(),
                DatabaseController.Instance.GetTable<RefererTable>(),
                DatabaseController.Instance.GetTable<UserAgentTable>(),
                DatabaseController.Instance.GetTable<LogEntryTable>()
            };
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Loads data according to the specified domain id.
        /// </summary>
        /// <param name="domain">The domain</param>
        public void Load(DomainRow domain)
        {
            /* Due to foreign keys, must clear in reverse order */
            for (int idx = tables.Count - 1; idx >= 0; idx--)
            {
                tables[idx].Clear();
            }

            /* Load in order  */
            for (int idx = 0; idx < tables.Count; idx++)
            {
                tables[idx].Load(domain);
            }
        }
        #endregion
    }
}

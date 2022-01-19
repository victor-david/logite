using Restless.Logite.Database.Tables;
using Restless.Toolkit.Core.Database.SQLite;
using Restless.Toolkit.Mvvm;
using System;
using System.Data;

namespace Restless.Logite.ViewModel.Domain
{
    public abstract class DomainController : DataGridViewModel<LogEntryTable>
    {
        /// <summary>
        /// Gets the domain object for this domain controller.
        /// </summary>
        public DomainRow Domain
        {
            get;
        }

        public DomainController(DomainRow domain)
        {
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
        }

        protected override void OnActivated()
        {

        }

        protected override bool OnDataRowFilter(DataRow item)
        {
            return Domain != null && (long)item[LogEntryTable.Defs.Columns.DomainId] == Domain.Id;
        }

        protected bool OnAdditionalDataRowFilter(DataRow item)
        {
            return !item[LogEntryTable.Defs.Columns.Calculated.Request].ToString().StartsWith("/asset", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

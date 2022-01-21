using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using Restless.Toolkit.Controls;
using System;
using System.Data;
using System.Diagnostics;

namespace Restless.Logite.ViewModel.Domain
{
    /// <summary>
    /// Provides the logic that is used to display stats for a specified domain.
    /// </summary>
    public class DomainViewModel : ApplicationViewModel
    {
        #region Private
        private string domainStatus;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the domain object for this domain view.
        /// </summary>
        public DomainRow Domain
        {
            get;
        }

        public string DomainStatus
        {
            get => domainStatus;
            private set => SetProperty(ref domainStatus, value);
        }

        /// <summary>
        /// Gets the method controller.
        /// </summary>
        public MethodController Method
        {
            get;
        }

        /// <summary>
        /// Gets the status controller.
        /// </summary>
        public StatusController Status
        {
            get;
        }

        /// <summary>
        /// Gets the ip address controller.
        /// </summary>
        public IpAddressController Ip
        {
            get;
        }

        /// <summary>
        /// Gets the log entry controller
        /// </summary>
        public LogEntryController LogEntry
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainViewModel"/> class.
        /// </summary>
        public DomainViewModel(DomainRow domain) : base()
        {
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
            DisplayName = Domain.DisplayName;

            Method = new MethodController(Domain);
            Status = new StatusController(Domain);
            Ip = new IpAddressController(Domain);
            LogEntry = new LogEntryController(Domain);

            Method.SelectedItemChanged += (s, id) => LogEntry.UpdateFilter(LogEntryTable.Defs.Columns.MethodId, id);
            Status.SelectedItemChanged += (s, id) => LogEntry.UpdateFilter(LogEntryTable.Defs.Columns.Status, id);
            Ip.SelectedItemChanged += (s, id) => LogEntry.UpdateFilter(LogEntryTable.Defs.Columns.IpAddressId, id);

            UpdateDomainStatus();
        }
        #endregion

        /************************************************************************/

        #region Protected Methods
        protected override void OnActivated()
        {
            DemandDomainController.Instance.Load(Domain.Id);
            Method.Activate();
            Status.Activate();
            Ip.Activate();
            LogEntry.Activate();
            UpdateDomainStatus();
        }

        protected override void OnDeactivated()
        {
            LogEntry.Deactivate();
            Method.Deactivate();
            Status.Deactivate();
            Ip.Deactivate();

        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void UpdateDomainStatus()
        {
            DomainStatus = $"{Domain.LogEntryCount} log entries";
        }
        #endregion
    }
}
using Restless.Logite.Core;
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using Restless.Logite.Resources;
using Restless.Toolkit.Controls;
using System;
using System.Collections.Generic;
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

        public ChartController Chart
        {
            get;
        }

        /// <summary>
        /// Gets the filter controller
        /// </summary>
        public FilterController Filter
        {
            get;
        }

        /// <summary>
        /// Gets the section selector
        /// </summary>
        public SectionSelector Sections
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
            Chart = new ChartController(Domain);

            Sections = new SectionSelector()
            {
                TitlePreface = Strings.TextData
            };
            InitializeSections();

            Filter = new FilterController()
            {
                TitlePreface = Strings.TextPeriod,
                DateTimeColumnName = LogEntryTable.Defs.Columns.Timestamp,
                TextSearchColumnNames = new string[]
                {
                    LogEntryTable.Defs.Columns.Calculated.Request
                }
            };

            Filter.SetSelectedTimeFilter((TimeFilter)Domain.Period);

            Filter.FilterChanged += (s, e) =>
            {
                Domain.Period = (long)e.Item.Filter;
                Domain.Table.Save();
                Update();
            };

            Method.SelectedItemChanged += (s, id) => LogEntry.UpdateFilter(LogEntryFilterType.Method, id);
            Status.SelectedItemChanged += (s, id) => LogEntry.UpdateFilter(LogEntryFilterType.Status, id);
            Ip.SelectedItemChanged += (s, id) => LogEntry.UpdateFilter(LogEntryFilterType.IpAddress, id);

            UpdateDomainStatus();
        }
        #endregion

        /************************************************************************/

        #region Protected Methods
        /// <summary>
        /// Called when the view model is activated.
        /// </summary>
        /// <remarks>
        /// Activation occurs for <see cref="DomainViewModel"/> when the user selects
        /// a domain from the navigation list. This method sets <see cref="LogEntry"/>
        /// to an activated state and calls update to load the domain records.
        /// Note that <see cref="LogEntry"/> doesn't do anything upon activation,
        /// but it needs to be placed into an active state so that it can be 
        /// deactivated; the Deactivate() method doesn't do anything if the
        /// view model isn't in an activate state.
        /// </remarks>
        protected override void OnActivated()
        {
            LogEntry.Activate();
            Update();
            UpdateDomainStatus();
        }

        protected override void OnUpdate()
        {
            DatabaseController.Instance.GetTable<LogEntryTable>().UnloadDomain();

            if (Domain.DisplayMode == DomainTable.Defs.Values.DisplayMode.Raw)
            {
                DatabaseController.Instance.GetTable<LogEntryTable>().LoadDomain(Domain);
                UpdateControllers();

            }
            else
            {
                UpdateControllers();
                Chart.Update();
            }
        }

        /// <summary>
        /// Deactivates the view model.
        /// </summary>
        /// <remarks>
        /// Deactivation occurs for <see cref="DomainViewModel"/> when the user switches to another
        /// domain or to another view model (such as settings). Of the sub-controllers owned
        /// by this class, only <see cref="LogEntry"/> currently needs to be deactivated.
        /// </remarks>
        protected override void OnDeactivated()
        {
            LogEntry.Deactivate();
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void InitializeSections()
        {
            Sections.Add(DomainTable.Defs.Values.DisplayMode.Raw, "Raw");
            Sections.Add(DomainTable.Defs.Values.DisplayMode.Chart, "Charts");
            Sections.SetSelectedSection(Domain.DisplayMode);
            Sections.SectionChanged += (s, e) =>
            {
                Domain.DisplayMode = e.Id;
                OnPropertyChanged(nameof(Domain));
                Update();
            };
        }

        private void UpdateControllers()
        {
            Method.Update();
            Status.Update();
            Ip.Update();
            LogEntry.Update();
        }

        private void UpdateDomainStatus()
        {
            DomainStatus = $"{Domain.LogEntryCount} log entries";
        }
        #endregion
    }
}
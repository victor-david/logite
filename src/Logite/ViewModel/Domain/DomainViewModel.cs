using Restless.Logite.Core;
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using Restless.Logite.Resources;
using System;

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
        protected override void OnActivated()
        {
            ActivateControllers();
            Update();
            UpdateDomainStatus();
        }

        protected override void OnUpdate()
        {
            DatabaseController.Instance.GetTable<LogEntryTable>().UnloadDomain();

            switch (Domain.DisplayMode)
            {
                case DomainTable.Defs.Values.DisplayMode.Raw:
                    DatabaseController.Instance.GetTable<LogEntryTable>().LoadDomain(Domain);
                    break;
                case DomainTable.Defs.Values.DisplayMode.Chart:
                    Chart.Update();
                    break;
            }
        }

        /// <summary>
        /// Deactivates the view model.
        /// </summary>
        protected override void OnDeactivated()
        {
            DeactivateControllers();
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

        private void ActivateControllers()
        {
            Method.Activate();
            Status.Activate();
            Ip.Activate();
            LogEntry.Activate();
        }

        private void DeactivateControllers()
        {
            Method.Deactivate();
            Status.Deactivate();
            Ip.Deactivate();
            LogEntry.Activate();
        }

        private void UpdateDomainStatus()
        {
            DomainStatus = $"{Domain.LogEntryCount} log entries";
        }
        #endregion
    }
}
using Restless.Logite.Core;
using Restless.Logite.Database.Tables;
using Restless.Logite.Resources;
using Restless.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace Restless.Logite.ViewModel.Domain
{
    public class LogEntryController : DomainController<LogEntryTable, LogEntryRow>, IDetailPanel
    {
        #region Private
        private Dictionary<LogEntryFilterType, long> filters;
        private double detailMinWidth;
        private GridLength detailWidth;
        #endregion

        /************************************************************************/

        #region Properties
        #endregion

        /************************************************************************/

        #region IDetailPanel
        /// <summary>
        /// Gets or sets a value that determines if the detail panel is visible.
        /// </summary>
        public bool IsDetailVisible
        {
            get => Config.LogEntryDetailVisible;
            set
            {
                Config.LogEntryDetailVisible = value;
                if (value)
                {
                    DetailMinWidth = Config.DetailPanel.DomainMinWidth;
                    DetailWidth = new GridLength(Config.LogEntryDetailWidth, GridUnitType.Pixel);
                }
                else
                {
                    DetailMinWidth = 0.0;
                    DetailWidth = new GridLength(0.0, GridUnitType.Pixel);
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the minimum width for detail panel.
        /// </summary>
        public double DetailMinWidth
        {
            get => detailMinWidth;
            private set => SetProperty(ref detailMinWidth, value);
        }

        /// <summary>
        /// Gets the maximum width for detail panel.
        /// </summary>
        public double DetailMaxWidth => Config.DetailPanel.LogEntryMaxWidth;


        /// <summary>
        /// Gets or sets the width of the detail panel.
        /// </summary>
        public GridLength DetailWidth
        {
            get => detailWidth;
            set
            {
                SetProperty(ref detailWidth, value);
                if (value.Value >= Config.DetailPanel.DomainMinWidth)
                {
                    Config.LogEntryDetailWidth = (int)value.Value;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntryController"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        public LogEntryController(DomainRow domain): base(domain)
        {
            Columns.Create("Id", nameof(LogEntryRow.Id)).MakeFixedWidth(FixedWidth.W052);
            Columns.Create("Timestamp", nameof(LogEntryRow.Timestamp)).MakeDate(Config.LogDisplayFormat).MakeFixedWidth(FixedWidth.W136);
            Columns.Create("Ip", nameof(LogEntryRow.IpAddress)).MakeFixedWidth(FixedWidth.W096);
            Columns.Create("Method", nameof(LogEntryRow.Method)).MakeFixedWidth(FixedWidth.W076);
            Columns.Create("Request", nameof(LogEntryRow.Request));
            Columns.Create("Status", nameof(LogEntryRow.Status))
                .AddCellStyle(LocalResources.Styles.StatusTextBlockStyle)
                .MakeFixedWidth(FixedWidth.W052);
            Columns.Create("Bytes", nameof(LogEntryRow.BytesSent)).MakeFixedWidth(FixedWidth.W052);
            filters = new Dictionary<LogEntryFilterType, long>();
            IsDetailVisible = Config.LogEntryDetailVisible;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Updates the log entry filter
        /// </summary>
        /// <param name="propertyName">The name of the (long) property to filter on</param>
        /// <param name="value">The value</param>
        public void UpdateFilter(LogEntryFilterType filterType, long value)
        {
            if (value != -1)
            {
                if (filters.ContainsKey(filterType))
                {
                    filters[filterType] = value;
                }
                else
                {
                    filters.Add(filterType, value);
                }
            }
            else
            {
                if (filters.ContainsKey(filterType))
                {
                    filters.Remove(filterType);
                }
            }
            ListView.Refresh();
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        protected override bool OnDataRowFilter(LogEntryRow item)
        {
            return 
                !item.Request.StartsWith("/asset", StringComparison.InvariantCultureIgnoreCase) &&
                EvaluateFilters(item);
        }

        protected override int OnDataRowCompare(LogEntryRow item1, LogEntryRow item2)
        {
            return item2.Timestamp.CompareTo(item1.Timestamp);
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private bool EvaluateFilters(LogEntryRow item)
        {
            bool result = true;
            if (filters != null)
            {
                foreach (var x in filters)
                {
                    result = result && GetFilterResult(x.Key, item, x.Value);
                }
            }
            return result;
        }

        private bool GetFilterResult(LogEntryFilterType filterType, LogEntryRow row, long value)
        {
            return filterType switch
            {
                LogEntryFilterType.IpAddress => row.IpAddressId == value,
                LogEntryFilterType.Method => row.MethodId == value,
                LogEntryFilterType.Status => row.Status == value,
                _ => true
            };
        }
        #endregion
    }
}
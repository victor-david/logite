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
        private Dictionary<string, long> filters;
        private double detailMinWidth;
        private GridLength detailWidth;
        private LogEntryRow logEntry;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the selected log entry
        /// </summary>
        public LogEntryRow LogEntry
        {
            get => logEntry;
            private set => SetProperty(ref logEntry, value);
        }
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
                    SynchronizeOnDetailVisible();
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
            //Columns.Create("Id", LogEntryTable.Defs.Columns.Id).MakeFixedWidth(FixedWidth.W052);
            //Columns.Create("Timestamp", LogEntryTable.Defs.Columns.Timestamp).MakeDate(Config.LogDisplayFormat).MakeFixedWidth(FixedWidth.W136);
            //Columns.Create("Ip", LogEntryTable.Defs.Columns.Calculated.IpAddress).MakeFixedWidth(FixedWidth.W096);
            //Columns.Create("Method", LogEntryTable.Defs.Columns.Calculated.Method).MakeFixedWidth(FixedWidth.W076);
            //Columns.Create("Request", LogEntryTable.Defs.Columns.Calculated.Request);
            //Columns.Create("Status", LogEntryTable.Defs.Columns.Status)
            //    .AddCellStyle(LocalResources.Styles.StatusTextBlockStyle)
            //    .MakeFixedWidth(FixedWidth.W052);
            //Columns.Create("Bytes", LogEntryTable.Defs.Columns.BytesSent).MakeFixedWidth(FixedWidth.W052);
            filters = new Dictionary<string, long>();
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
        public void UpdateFilter(string propertyName, long value)
        {
            if (value != -1)
            {
                if (filters.ContainsKey(propertyName))
                {
                    filters[propertyName] = value;
                }
                else
                {
                    filters.Add(propertyName, value);
                }
            }
            else
            {
                if (filters.ContainsKey(propertyName))
                {
                    filters.Remove(propertyName);
                }
            }
            //Refresh();
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when deactivated.
        /// </summary>
        /// <remarks>
        /// On deactivation, need to set <see cref="LogEntry"/> to null. It gets set when detail
        /// is displayed, but when switching to another domain, the log entry table
        /// is populated on demand and the row represented by LogEntry no longer exists;
        /// it then throws an exception if it's accessed.
        /// </remarks>
        protected override void OnDeactivated()
        {
            LogEntry = null;
        }

        /// <summary>
        /// Called when updated
        /// </summary>
        /// <remarks>
        /// Set <see cref="LogEntry"/> to null as upon deactivation.
        /// </remarks>
        protected override void OnUpdate()
        {
            LogEntry = null;
            base.OnUpdate();
        }

        protected override bool OnDataRowFilter(RawRow item)
        {
            return true;
                //Domain != null &&
                //(long)item[LogEntryTable.Defs.Columns.DomainId] == Domain.Id &&
                //!item[LogEntryTable.Defs.Columns.Calculated.Request].ToString().StartsWith("/asset", StringComparison.InvariantCultureIgnoreCase) &&
                //EvaluateFilters(item);
        }

        protected override int OnDataRowCompare(RawRow item1, RawRow item2)
        {
            return 0; //  DataRowCompareDateTime(item2, item1, LogEntryTable.Defs.Columns.Timestamp);
        }

        protected override void OnSelectedItemChanged()
        {
            //if (SelectedDataRow != null && IsDetailVisible)
            //{
            //    LogEntry = new LogEntryRow(SelectedDataRow);
            //}
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void SynchronizeOnDetailVisible()
        {
            //if (SelectedDataRow != null)
            //{
            //    if (LogEntry == null || LogEntry.Row != SelectedDataRow)
            //    {
            //        LogEntry = new LogEntryRow(SelectedDataRow);
            //    }
            //}
        }

        private bool EvaluateFilters(DataRow item)
        {
            bool result = true;
            foreach (var x in filters)
            {
                result = result && (long)item[x.Key] == x.Value;
            }
            return result;
        }
        #endregion
    }
}
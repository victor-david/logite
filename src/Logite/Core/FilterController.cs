using Restless.Logite.Resources;
using Restless.Toolkit.Mvvm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Provides properties and methods to implement domain filter operations.
    /// </summary>
    public class FilterController : ObservableObject
    {
        #region Private
        private string title;
        private string text;
        private TimeFilterItem selectedTimeFilter;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets or sets the preface that is applied to <see cref="Title"/>.
        /// </summary>
        public string TitlePreface
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the title used for the time filter drop down.
        /// </summary>
        public string Title
        {
            get => title;
            private set => SetProperty(ref title, value);
        }

        /// <summary>
        /// Gets or sets the text used to filter the domain log entries.
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                if (SetProperty(ref text, value))
                {
                    OnFilterChanged();
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected time filter item
        /// </summary>
        public TimeFilterItem SelectedTimeFilter
        {
            get => selectedTimeFilter;
            set
            {
                if (SetProperty(ref selectedTimeFilter, value) && selectedTimeFilter != null)
                {
                    UpdateTimeFilterTitle();
                    OnFilterChanged();
                }
            }
        }

        /// <summary>
        /// Gets the time filter options list.
        /// </summary>
        public List<TimeFilterItem> TimeFilters
        {
            get;
        }

        /// <summary>
        /// Gets or sets the name of the DateTime column to use when evaluating
        /// whether the item passed to <see cref="IsItemIncluded(DataRow)"/>
        /// is included in the date portion of the filter.
        /// </summary>
        public string DateTimeColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name(s) of the text columns to use when evaluating
        /// whether the item passed to <see cref="IsItemIncluded(DataRow)"/>
        /// is included in the text portion of the filter.
        /// </summary>
        public string[] TextSearchColumnNames
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the command that clears the text search.
        /// </summary>
        public ICommand ClearSearchTextCommand
        {
            get;
        }

        /// <summary>
        /// Raised when filter conditions changed.
        /// </summary>
        public event EventHandler<FilterChangedEventArgs> FilterChanged;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterController"/> class.
        /// </summary>
        public FilterController()
        {
            ClearSearchTextCommand = RelayCommand.Create(p => Text = null);

            TimeFilters = new List<TimeFilterItem>()
            {
                new TimeFilterItem(Strings.FilterDays003, TimeFilter.Days003),
                new TimeFilterItem(Strings.FilterDays007, TimeFilter.Days007),
                new TimeFilterItem(Strings.FilterDays014, TimeFilter.Days014),
                new TimeFilterItem(Strings.FilterDays030, TimeFilter.Days030),
                new TimeFilterItem(Strings.FilterDays060, TimeFilter.Days060),
                new TimeFilterItem(Strings.FilterDays090, TimeFilter.Days090),
                new TimeFilterItem(Strings.FilterDays120, TimeFilter.Days120),
                new TimeFilterItem(Strings.FilterDays180, TimeFilter.Days180),
                new TimeFilterItem(Strings.FilterYears1, TimeFilter.Years1),
                new TimeFilterItem(Strings.FilterYears2, TimeFilter.Years2),
            };

            UpdateTimeFilterTitle();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Sets <see cref="SelectedTimeFilter"/> to the item that contains the specified filter.
        /// </summary>
        /// <param name="filter">The filter value.</param>
        public void SetSelectedTimeFilter(TimeFilter filter)
        {
            foreach (TimeFilterItem item in TimeFilters)
            {
                if (item.Filter == filter)
                {
                    SelectedTimeFilter = item;
                }
            }
        }

        /// <summary>
        /// Gets a boolean value that indicates whether the specified item
        /// should be included based on the current filter conditions.
        /// </summary>
        /// <param name="item">The data row item</param>
        /// <returns>true if the item should be included; otherwise, false;</returns>
        public bool IsItemIncluded(DataRow item)
        {
            return IsPresetFilterIncluded(item) && IsUserFilterIncluded(item);
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Raises the <see cref="FilterChanged"/> event.
        /// </summary>
        protected virtual void OnFilterChanged(FilterChangedEventArgs e)
        {
            FilterChanged?.Invoke(this, e);
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private void OnFilterChanged()
        {
            OnFilterChanged(new FilterChangedEventArgs(SelectedTimeFilter, Text));
        }

        private bool IsPresetFilterIncluded(DataRow item)
        {
            return selectedTimeFilter != null && selectedTimeFilter.Filter switch
            {
                TimeFilter.All => true,
                TimeFilter.Days003 => IsPresetDateIncluded(item, -3),
                TimeFilter.Days007 => IsPresetDateIncluded(item, -7),
                TimeFilter.Days014 => IsPresetDateIncluded(item, -14),
                TimeFilter.Days030 => IsPresetDateIncluded(item, -30),
                TimeFilter.Days060 => IsPresetDateIncluded(item, -60),
                TimeFilter.Days090 => IsPresetDateIncluded(item, -90),
                TimeFilter.Days120 => IsPresetDateIncluded(item, -120),
                TimeFilter.Days180 => IsPresetDateIncluded(item, -180),
                TimeFilter.Years1 => IsPresetDateIncluded(item, -365),
                TimeFilter.Years2 => IsPresetDateIncluded(item, -365 * 2),
                _ => false,
            };
        }

        private bool IsPresetDateIncluded(DataRow item, int pastDays)
        {
            return IsPresetDateIncluded(item, DateTime.UtcNow.AddDays(pastDays).Date);
        }

        private bool IsPresetDateIncluded(DataRow item, DateTime startDate)
        {
            return !string.IsNullOrEmpty(DateTimeColumnName) && ((DateTime)item[DateTimeColumnName]).CompareTo(startDate) >= 0;
        }

        private bool IsUserFilterIncluded(DataRow item)
        {
            if (!string.IsNullOrWhiteSpace(text) && TextSearchColumnNames != null)
            {
                foreach (string columnName in TextSearchColumnNames)
                {
                    if (item[columnName].ToString().Contains(text, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        private void UpdateTimeFilterTitle()
        {
            Title = $"{TitlePreface}: {SelectedTimeFilter?.Name}";
        }
        #endregion
    }
}
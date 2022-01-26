using Restless.Controls.Chart;
using Restless.Logite.Database.Tables;
using Restless.Toolkit.Mvvm;
using System;
using System.Threading;

namespace Restless.Logite.ViewModel.Domain
{
    /// <summary>
    /// Base data provider for charts. This class must be inherited.
    /// </summary>
    public abstract class ChartDataProvider : ObservableObject
    {
        #region Private
        private DataSeries data;
        private string yAxisTextFormat;
        private object selectedLegendItem;
        private int selectedLegendIndex;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the domain associated with this chart data
        /// </summary>
        public DomainRow Domain
        {
            get;
        }

        /// <summary>
        /// Gets the data series for the chart data
        /// </summary>
        public DataSeries Data
        {
            get => data;
            protected set => SetProperty(ref data, value);
        }

        /// <summary>
        /// Gets the converter that returns date strings for the charts.
        /// </summary>
        public MappedValueConverter XAxisDateConverter
        {
            get;
        }

        /// <summary>
        /// Gets the converter that is used to provide strings for the Y axis.
        /// </summary>
        public IDoubleConverter YAxisConverter
        {
            get;
        }

        /// <summary>
        /// Gets the text format to use for the Y axis of the charts.
        /// </summary>
        public string YAxisTextFormat
        {
            get => yAxisTextFormat;
            protected set => SetProperty(ref yAxisTextFormat, value);
        }

        protected abstract double YAxisDivisor { get; }

        public int SelectedLegendIndex
        {
            get => selectedLegendIndex;
            set => SetProperty(ref selectedLegendIndex, value);
        }

        public object SelectedLegendItem
        {
            get => selectedLegendItem;
            set
            {
                if (SetProperty(ref selectedLegendItem, value))
                {
                    OnSelectedLegendItemChanged(selectedLegendItem as DataSeriesInfo);
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDataProvider"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        protected ChartDataProvider(DomainRow domain)
        {
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
            XAxisDateConverter = new MappedValueConverter();
            YAxisConverter = new DivisionConverter(YAxisDivisor);
            YAxisTextFormat = "N0";
            SelectedLegendIndex = -1;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        public abstract void CreateData();
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when <see cref="SelectedLegendItem"/> changes. 
        /// The base implementation sets <see cref="SelectedLegendIndex"/>
        /// from <paramref name="info"/>. Override in a derived class to take
        /// other action.
        /// </summary>
        /// <param name="info">The info. May be null.</param>
        protected virtual void OnSelectedLegendItemChanged(DataSeriesInfo info)
        {
            SelectedLegendIndex = (info != null) ? info.Index : -1;
        }

        /// <summary>
        /// Gets a string from the specified x-axis date
        /// </summary>
        /// <param name="xAxisDate">The x-axis date</param>
        /// <returns>A string representation of the date.</returns>
        protected virtual string GetXAxisDateString(DateTime xAxisDate)
        {
            return xAxisDate.ToString("MMM dd\nyyyy", Thread.CurrentThread.CurrentCulture);
        }
        #endregion
    }
}

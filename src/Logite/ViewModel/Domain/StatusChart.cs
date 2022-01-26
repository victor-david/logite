using Restless.Controls.Chart;
using Restless.Logite.Core;
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using Restless.Logite.Resources;
using System.Windows.Media;

namespace Restless.Logite.ViewModel.Domain
{
    /// <summary>
    /// Provides data for the status code chart
    /// </summary>
    public class StatusChart : ChartDataProvider
    {
        #region Properties
        protected override double YAxisDivisor => 1.0;

        /// <summary>
        /// Gets the status selector
        /// </summary>
        public StatusSelector Selector
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TrafficChart"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        public StatusChart(DomainRow domain): base(domain)
        {
            Selector = new StatusSelector();
            Selector.TitlePreface = Strings.ChartTitleStatusCodes;
            Selector.AddStatus("200", 200, (long)ChartStatus.Code200, Brushes.MediumSeaGreen);
            Selector.AddStatus("302", 302, (long)ChartStatus.Code302, Brushes.DodgerBlue);
            Selector.AddStatus("304", 304, (long)ChartStatus.Code304, Brushes.CornflowerBlue);
            Selector.AddStatus("400", 400, (long)ChartStatus.Code400, Brushes.DarkMagenta);
            Selector.AddStatus("404", 404, (long)ChartStatus.Code404, Brushes.Coral);
            Selector.AddStatus("444", 444, (long)ChartStatus.Code444, Brushes.Red);
            Selector.AddStatus("500", 500, (long)ChartStatus.Code500, Brushes.DarkOrange);
            Selector.AddSeparator();
            Selector.AddClearStatus(Strings.TextClearAll);
            Selector.Initialize(domain.ChartStatus);
            Selector.OnSelectedStatusChanged = SelectedStatusChanged;
            Selector.OnClearAllStatus = ClearAllStatus;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Creates and assigns the chart data
        /// </summary>
        public override void CreateData()
        {
            int seriesCount = Selector.SelectionCount;
            if (seriesCount == 0)
            {
                Data = DataSeries.Create(0);
                return;
            }

            DataSeries data = DataSeries.Create(seriesCount);
            XAxisDateConverter.ClearMap();

            int x = 1;
            DataPointCollection<StatusDataPoint> dataPoints = DatabaseController.Instance.GetTable<LogEntryTable>().GetStatusTrafficData(Domain);
            foreach (StatusDataPoint point in dataPoints)
            {
                foreach (StatusSelection selection in Selector.EnumerateSelected())
                {
                    data.Add(x, GetPointPropertyFromValue(point, selection.Value));
                }
                XAxisDateConverter.AddToMap(x, GetXAxisDateString(point.Date));
                x++;
            }

            int idx = 0;
            foreach (StatusSelection selection in Selector.EnumerateSelected())
            {
                data.DataInfo.SetInfo(idx++, selection.Name, selection.StatusBrush, Brushes.DarkGray);
            }

            data.ExpandX(1);
            data.DataRange.Y.IncreaseMaxBy(0.10);
            data.MakeYAutoZero();

            Data = data;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private long GetPointPropertyFromValue(StatusDataPoint point, long value)
        {
            return value switch
            {
                200 => point.Count200,
                302 => point.Count302,
                304 => point.Count304,
                400 => point.Count400,
                404 => point.Count404,
                444 => point.Count444,
                500 => point.Count500,
                _ => 0
            };
        }

        private void SelectedStatusChanged(StatusSelection selection)
        {
            Domain.ChartStatus = Selector.BitValueSelectedSum;
            CreateData();
        }

        private void ClearAllStatus()
        {
            Domain.ChartStatus = 0;
            CreateData();
        }
        #endregion
    }
}
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
            Selector.AddStatus("200", StatusCode.Code200.Value, StatusCode.Code200.BitValue, Brushes.MediumSeaGreen);
            Selector.AddStatus("302", StatusCode.Code302.Value, StatusCode.Code302.BitValue, Brushes.DodgerBlue);
            Selector.AddStatus("304", StatusCode.Code304.Value, StatusCode.Code304.BitValue, Brushes.CornflowerBlue);
            Selector.AddStatus("400", StatusCode.Code400.Value, StatusCode.Code400.BitValue, Brushes.DarkMagenta);
            Selector.AddStatus("404", StatusCode.Code404.Value, StatusCode.Code404.BitValue, Brushes.Coral);
            Selector.AddStatus("444", StatusCode.Code444.Value, StatusCode.Code444.BitValue, Brushes.Red);
            Selector.AddStatus("500", StatusCode.Code500.Value, StatusCode.Code500.BitValue, Brushes.DarkOrange);
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
                    data.Add(x, point.GetCountForStatus(selection.Value));
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
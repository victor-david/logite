using Restless.Controls.Chart;
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using System.Windows.Media;

namespace Restless.Logite.ViewModel.Domain
{
    /// <summary>
    /// Provides data for the unique ip address chart.
    /// </summary>
    public class UniqueIpChart : ChartDataProvider
    {
        protected override double YAxisDivisor => 1.0;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueIpChart"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        public UniqueIpChart(DomainRow domain): base(domain)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Creates and assigns the chart data
        /// </summary>
        public override void CreateData()
        {
            DataSeries data = DataSeries.Create();
            XAxisDateConverter.ClearMap();

            int x = 1;
            DataPointCollection<CountDataPoint> dataPoints = DatabaseController.Instance.GetTable<LogEntryTable>().GetUniqueIpTrafficData(Domain);
            foreach (CountDataPoint point in dataPoints)
            {
                data.Add(x, point.Count);
                XAxisDateConverter.AddToMap(x, GetXAxisDateString(point.Date));
                x++;
            }

            data.DataInfo.SetInfo(0, "Ip", Brushes.DarkCyan, Brushes.DarkGray);
            data.ExpandX(1);
            data.DataRange.Y.IncreaseMaxBy(0.10);
            data.MakeYAutoZero();

            Data = data;
        }
        #endregion
    }
}
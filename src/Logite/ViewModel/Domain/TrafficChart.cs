using Restless.Controls.Chart;
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Restless.Logite.ViewModel.Domain
{
    public class TrafficChart : ChartBase
    {
        protected override double YAxisDivisor => 1.0;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TrafficChart"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        public TrafficChart(DomainRow domain): base(domain)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        public override void CreateData()
        {
            DataSeries data = DataSeries.Create();
            XAxisDateConverter.ClearMap();

            int x = 1;
            DataPointCollection<CountDataPoint> dataPoints = DatabaseController.Instance.GetTable<LogEntryTable>().GetTotalTrafficData(Domain);
            foreach (CountDataPoint point in dataPoints)
            {
                data.Add(x, point.Count);
                XAxisDateConverter.AddToMap(x, GetXAxisDateString(point.Date));
                x++;
            }

            data.DataInfo.SetInfo(0, "Traffic", Brushes.LightSkyBlue, Brushes.DarkGray);
            data.ExpandX(1);
            data.DataRange.Y.IncreaseMaxBy(0.10);
            data.MakeYAutoZero();

            Data = data;
        }
        #endregion
    }
}

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
    public class StatusChart : ChartBase
    {
        protected override double YAxisDivisor => 1.0;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TrafficChart"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        public StatusChart(DomainRow domain): base(domain)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        public override void CreateData()
        {
            DataSeries data = DataSeries.Create(2);
            XAxisDateConverter.ClearMap();

            int x = 1;
            DataPointCollection<StatusDataPoint> dataPoints = DatabaseController.Instance.GetTable<LogEntryTable>().GetStatusTrafficData(Domain, 200);
            foreach (StatusDataPoint point in dataPoints)
            {
                data.Add(x, point.Count200);
                data.Add(x, point.Count404);
                XAxisDateConverter.AddToMap(x, GetXAxisDateString(point.Date));
                x++;
            }

            data.DataInfo.SetInfo(0, "200", Brushes.ForestGreen, Brushes.DarkGray);
            // data.DataInfo.SetInfo(1, "400+", Brushes.OrangeRed, Brushes.DarkGray);
            data.DataInfo.SetInfo(1, "404", Brushes.IndianRed, Brushes.DarkGray);
            // data.DataInfo.SetInfo(3, "444", Brushes.Red, Brushes.DarkGray);


            data.ExpandX(1);
            data.DataRange.Y.IncreaseMaxBy(0.10);
            data.MakeYAutoZero();

            Data = data;
        }
        #endregion
    }
}
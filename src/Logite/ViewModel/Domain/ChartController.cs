using Restless.Logite.Database.Tables;
using System;

namespace Restless.Logite.ViewModel.Domain
{
    public class ChartController : ApplicationViewModel
    {
        #region Properties
        /// <summary>
        /// Gets the domain associated with this chart controller
        /// </summary>
        public DomainRow Domain
        {
            get;
        }

        /// <summary>
        /// Gets the traffic chart data provider.
        /// </summary>
        public TrafficChart Traffic
        {
            get;
        }

        /// <summary>
        /// Gets the status chart data provider.
        /// </summary>
        public StatusChart Status
        {
            get;
        }

        /// <summary>
        /// Gets the unique ip chart data provider.
        /// </summary>
        public UniqueIpChart UniqueIp
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartController"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        public ChartController(DomainRow domain)
        {
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
            Traffic = new TrafficChart(Domain);
            Status = new StatusChart(Domain);
            UniqueIp = new UniqueIpChart(Domain);
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when this controller is updated to create the chart data.
        /// </summary>
        protected override void OnUpdate()
        {
            Traffic.CreateData();
            Status.CreateData();
            UniqueIp.CreateData();
        }
        #endregion
    }
}
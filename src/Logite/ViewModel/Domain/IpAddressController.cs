using Restless.Logite.Core;
using Restless.Logite.Database.Tables;
using Restless.Toolkit.Controls;

namespace Restless.Logite.ViewModel.Domain
{
    public class IpAddressController : DomainController<IpAddressTable, IpAddressRow>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="IpAddressController"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        public IpAddressController(DomainRow domain): base(domain)
        {
            Columns.Create("Id", nameof(IpAddressRow.Id)).MakeFixedWidth(FixedWidth.W048);
            Columns.Create("Ip Address", nameof(IpAddressRow.IpAddress));
            Columns.Create("Count", nameof(IpAddressRow.UsageCount)).MakeFixedWidth(FixedWidth.W096);
        }
        #endregion

        /************************************************************************/

        #region Protected methods

        protected override int OnDataRowCompare(IpAddressRow item1, IpAddressRow item2)
        {
            return item2.UsageCount.CompareTo(item1.UsageCount);
        }

        protected override void OnSelectedItemChanged()
        {
            long id = (SelectedRawRow != null) ? SelectedRawRow.Id : -1;
            OnSelectedItemChanged(id);
        }
        #endregion
    }
}
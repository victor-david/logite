using Restless.Logite.Core;
using Restless.Logite.Database.Tables;
using Restless.Toolkit.Controls;
using System.Data;

namespace Restless.Logite.ViewModel.Domain
{
    public class IpAddressController : DomainController<IpAddressTable>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="IpAddressController"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        public IpAddressController(DomainRow domain): base(domain)
        {
            Columns.Create("Id", IpAddressTable.Defs.Columns.Id).MakeFixedWidth(FixedWidth.W048);
            Columns.Create("Ip Address", IpAddressTable.Defs.Columns.IpAddress);
            Columns.Create("Count", IpAddressTable.Defs.Columns.Calculated.UsageCount).MakeFixedWidth(FixedWidth.W096);
        }
        #endregion

        /************************************************************************/

        #region Protected methods

        protected override bool OnDataRowFilter(DataRow item)
        {
            return (long)item[IpAddressTable.Defs.Columns.Calculated.UsageCount] > 0;
        }

        protected override int OnDataRowCompare(DataRow item1, DataRow item2)
        {
            return DataRowCompareLong(item2, item1, IpAddressTable.Defs.Columns.Calculated.UsageCount);
        }

        protected override void OnSelectedItemChanged()
        {
            long id = (SelectedDataRow != null) ? (long)SelectedDataRow[IpAddressTable.Defs.Columns.Id] : -1;
            OnSelectedItemChanged(id);
        }
        #endregion
    }
}
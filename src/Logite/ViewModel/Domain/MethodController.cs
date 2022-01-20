using Restless.Logite.Core;
using Restless.Logite.Database.Tables;
using Restless.Toolkit.Controls;
using System.Data;

namespace Restless.Logite.ViewModel.Domain
{
    /// <summary>
    /// Display http methods (GET, POST, etc)
    /// </summary>
    public class MethodController : DomainController<MethodTable>
    {
        public MethodController(DomainRow domain) : base(domain)
        {
            Columns.Create("Method", MethodTable.Defs.Columns.Method);
            Columns.Create("Count", MethodTable.Defs.Columns.Calculated.UsageCount).MakeFixedWidth(FixedWidth.W096);
        }

        protected override void OnActivated()
        {
            SelectedItem = null;
            Refresh();
        }

        protected override bool OnDataRowFilter(DataRow item)
        {
            return (long)item[MethodTable.Defs.Columns.Calculated.UsageCount] > 0;
        }

        protected override int OnDataRowCompare(DataRow item1, DataRow item2)
        {
            return DataRowCompareLong(item1, item2, MethodTable.Defs.Columns.Id);
        }

        protected override void OnSelectedItemChanged()
        {
            long id = (SelectedDataRow != null) ? (long)SelectedDataRow[MethodTable.Defs.Columns.Id] : -1;
            OnSelectedItemChanged(id);
        }
    }
}

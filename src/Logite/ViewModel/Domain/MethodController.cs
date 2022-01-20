using Restless.Logite.Core;
using Restless.Logite.Database.Tables;
using Restless.Toolkit.Controls;
using System.Data;

namespace Restless.Logite.ViewModel.Domain
{
    public class MethodController : DomainController<DomainMethodTable>
    {
        public MethodController(DomainRow domain) : base(domain)
        {
            Columns.Create("Method", DomainMethodTable.Defs.Columns.Calculated.Method);
            Columns.Create("Count", DomainMethodTable.Defs.Columns.UsageCount).MakeFixedWidth(FixedWidth.W096);
        }

        protected override void OnActivated()
        {
            SelectedItem = null;
            Refresh();
        }

        protected override bool OnDataRowFilter(DataRow item)
        {
            return 
                Domain != null &&
                (long)item[DomainMethodTable.Defs.Columns.DomainId] == Domain.Id;
        }

        protected override void OnSelectedItemChanged()
        {
            long id = (SelectedDataRow != null) ? (long)SelectedDataRow[DomainMethodTable.Defs.Columns.MethodId] : -1;
            OnSelectedItemChanged(id);
           
        }
    }
}

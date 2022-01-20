using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Logite.ViewModel.Domain
{
    public class MethodController : DomainController<DomainMethodTable>
    {
        public MethodController(DomainRow domain) : base(domain)
        {
            Columns.Create("Method", DomainMethodTable.Defs.Columns.Calculated.Method);
            Columns.Create("Count", DomainMethodTable.Defs.Columns.UsageCount);
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

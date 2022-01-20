using Restless.Logite.Core;
using Restless.Logite.Database.Tables;
using Restless.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Logite.ViewModel.Domain
{
    public class StatusController : DomainController<StatusTable>
    {
        public StatusController(DomainRow domain): base(domain)
        {
            Columns.Create("Status", StatusTable.Defs.Columns.Status); //.MakeFixedWidth(FixedWidth.W052);
            Columns.Create("Count", StatusTable.Defs.Columns.Calculated.UsageCount); //.MakeFixedWidth(FixedWidth.W052);
        }

        protected override void OnActivated()
        {
            SelectedItem = null;
            Refresh();
        }

        protected override bool OnDataRowFilter(DataRow item)
        {
            return (long)item[StatusTable.Defs.Columns.Calculated.UsageCount] > 0;
        }

        protected override int OnDataRowCompare(DataRow item1, DataRow item2)
        {
            return DataRowCompareLong(item1, item2, StatusTable.Defs.Columns.Status);
        }

        protected override void OnSelectedItemChanged()
        {
            long id = (SelectedDataRow != null) ? (long)SelectedDataRow[StatusTable.Defs.Columns.Status] : -1;
            OnSelectedItemChanged(id);
        }
    }
}

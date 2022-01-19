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
    public class LogEntryController : DomainController
    {
        public LogEntryController(DomainRow domain): base(domain)
        {
            Columns.Create("Timestamp", LogEntryTable.Defs.Columns.Timestamp).MakeDate(Config.LogDisplayFormat).MakeFixedWidth(FixedWidth.W136);
            Columns.Create("Ip", LogEntryTable.Defs.Columns.RemoteAddress).MakeFixedWidth(FixedWidth.W096);
            Columns.Create("Method", LogEntryTable.Defs.Columns.Calculated.Method).MakeFixedWidth(FixedWidth.W052);
            Columns.Create("Request", LogEntryTable.Defs.Columns.Calculated.Request);
            Columns.Create("Status", LogEntryTable.Defs.Columns.Status).MakeFixedWidth(FixedWidth.W052);
            Columns.Create("Bytes", LogEntryTable.Defs.Columns.BytesSent).MakeFixedWidth(FixedWidth.W052);
        }

        protected override void OnActivated()
        {
            Refresh();
        }

        protected override bool OnDataRowFilter(DataRow item)
        {
            return base.OnDataRowFilter(item) && OnAdditionalDataRowFilter(item);
        }
    }
}

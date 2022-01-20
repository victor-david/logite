using Restless.Logite.Core;
using Restless.Logite.Database.Tables;
using Restless.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Data;

namespace Restless.Logite.ViewModel.Domain
{
    public class LogEntryController : DomainController<LogEntryTable>
    {
        private Dictionary<string, long> filters;

        public LogEntryController(DomainRow domain): base(domain)
        {
            Columns.Create("Id", LogEntryTable.Defs.Columns.DomainId).MakeFixedWidth(FixedWidth.W032);
            Columns.Create("Timestamp", LogEntryTable.Defs.Columns.Timestamp).MakeDate(Config.LogDisplayFormat).MakeFixedWidth(FixedWidth.W136);
            Columns.Create("Ip", LogEntryTable.Defs.Columns.Calculated.IpAddress).MakeFixedWidth(FixedWidth.W096);
            Columns.Create("Method", LogEntryTable.Defs.Columns.Calculated.Method).MakeFixedWidth(FixedWidth.W076);
            Columns.Create("Request", LogEntryTable.Defs.Columns.Calculated.Request);
            Columns.Create("Status", LogEntryTable.Defs.Columns.Status).MakeFixedWidth(FixedWidth.W052);
            Columns.Create("Bytes", LogEntryTable.Defs.Columns.BytesSent).MakeFixedWidth(FixedWidth.W052);
            filters = new Dictionary<string, long>();
        }

        public void UpdateFilter(string propertyName, long value)
        {
            if (value != -1)
            {
                if (filters.ContainsKey(propertyName))
                {
                    filters[propertyName] = value;
                }
                else
                {
                    filters.Add(propertyName, value);
                }
            }
            else
            {
                if (filters.ContainsKey(propertyName))
                {
                    filters.Remove(propertyName);
                }
            }
            Refresh();
        }


        protected override void OnActivated()
        {
            Refresh();
        }

        protected override void OnDeactivated()
        {
        }


        protected override bool OnDataRowFilter(DataRow item)
        {
            return
                Domain != null &&
                (long)item[LogEntryTable.Defs.Columns.DomainId] == Domain.Id &&
                !item[LogEntryTable.Defs.Columns.Calculated.Request].ToString().StartsWith("/asset", StringComparison.InvariantCultureIgnoreCase) &&
                EvaluateFilters(item);
        }

        protected override int OnDataRowCompare(DataRow item1, DataRow item2)
        {
            return DataRowCompareLong(item2, item1, LogEntryTable.Defs.Columns.Id);
        }

        private bool EvaluateFilters(DataRow item)
        {
            bool result = true;
            foreach (var x in filters)
            {
                result = result && (long)item[x.Key] == x.Value;
            }
            return result;
        }
    }
}

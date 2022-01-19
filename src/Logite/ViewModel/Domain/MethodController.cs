using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Logite.ViewModel.Domain
{
    public class MethodController : DomainController
    {
        public MethodController(DomainRow domain) : base(domain)
        {
            //Columns.Create("Method", MethodTable.Defs.Columns.Method);
            //Columns.Create("Count", MethodTable.Defs.Columns.Calculated.UsageCount);
            //Columns.Create("D Count", MethodTable.Defs.Columns.Calculated.DomainUsageCount);

            //DatabaseController.Instance.GetTable<MethodTable>().ComputeForDomain(Domain);

        }

        //protected override void OnActivated()
        //{
        //    DatabaseController.Instance.GetTable<MethodTable>().ComputeForDomain(Domain);
        //}
    }
}

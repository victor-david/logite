using Restless.Logite.Core;
using Restless.Logite.Database.Tables;
using Restless.Toolkit.Controls;
using System.Data;

namespace Restless.Logite.ViewModel.Domain
{
    /// <summary>
    /// Display http methods (GET, POST, etc)
    /// </summary>
    public class MethodController : DomainController<MethodTable, MethodRow>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodController"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        public MethodController(DomainRow domain) : base(domain)
        {
            //Columns.Create("Method", MethodTable.Defs.Columns.Method);
            //Columns.Create("Count", MethodTable.Defs.Columns.Calculated.UsageCount).MakeFixedWidth(FixedWidth.W096);
        }
        #endregion

        /************************************************************************/

        #region Protected methods

        protected override bool OnDataRowFilter(RawRow item)
        {
            return true; //  (long)item[MethodTable.Defs.Columns.Calculated.UsageCount] > 0;
        }

        protected override int OnDataRowCompare(RawRow item1, RawRow item2)
        {
            return 0; // DataRowCompareLong(item1, item2, MethodTable.Defs.Columns.Id);
        }

        protected override void OnSelectedItemChanged()
        {
            //long id = (SelectedDataRow != null) ? (long)SelectedDataRow[MethodTable.Defs.Columns.Id] : -1;
            //OnSelectedItemChanged(id);
        }
        #endregion
    }
}
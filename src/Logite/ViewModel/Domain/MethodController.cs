using Restless.Logite.Core;
using Restless.Logite.Database.Tables;
using Restless.Toolkit.Controls;

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
            Columns.Create("Method", nameof(MethodRow.Method));
            Columns.Create("Count", nameof(MethodRow.UsageCount)).MakeFixedWidth(FixedWidth.W096);
        }
        #endregion

        /************************************************************************/

        #region Protected methods

        protected override int OnDataRowCompare(MethodRow item1, MethodRow item2)
        {
            return item1.Id.CompareTo(item2.Id);
        }

        protected override void OnSelectedItemChanged()
        {
            long id = (SelectedRawRow != null) ? SelectedRawRow.Id : -1;
            OnSelectedItemChanged(id);
        }
        #endregion
    }
}
using Restless.Logite.Database.Tables;

namespace Restless.Logite.ViewModel.Domain
{
    public class StatusController : DomainController<StatusTable, StatusRow>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusController"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        public StatusController(DomainRow domain): base(domain)
        {
            Columns.Create("Status", nameof(StatusRow.Status)); //.MakeFixedWidth(FixedWidth.W052);
            Columns.Create("Count",  nameof(StatusRow.UsageCount)); //.MakeFixedWidth(FixedWidth.W052);
        }
        #endregion

        /************************************************************************/

        #region Protected methods

        protected override int OnDataRowCompare(StatusRow item1, StatusRow item2)
        {
            return item1.Status.CompareTo(item2.Status);
        }

        protected override void OnSelectedItemChanged()
        {
            long id = (SelectedRawRow != null) ? SelectedRawRow.Status : -1;
            OnSelectedItemChanged(id);
        }
        #endregion
    }
}

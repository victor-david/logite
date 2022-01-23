using Restless.Logite.Database.Tables;
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.ComponentModel;

namespace Restless.Logite.ViewModel.Domain
{
    public abstract class DomainController<T> : DataGridViewModel<T> where T:TableBase
    {
        #region Properties / Events
        /// <summary>
        /// Gets the domain object for this domain controller.
        /// </summary>
        public DomainRow Domain
        {
            get;
        }

        /// <summary>
        /// Occurs when the selected item changes
        /// </summary>
        public event EventHandler<long> SelectedItemChanged;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainController"/> class.
        /// </summary>
        /// <param name="domain">The domain</param>
        public DomainController(DomainRow domain)
        {
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
            Commands.Add("ClearSelection", p => SelectedItem = null);
        }
        #endregion

        /************************************************************************/

        #region Protected methods

        protected override void OnUpdate()
        {
            SelectedItem = null;
            Refresh();
        }

        /// <summary>
        /// Raises the <see cref="SelectedItemChanged"/> event
        /// </summary>
        /// <param name="id">The associated id</param>
        protected void OnSelectedItemChanged(long id)
        {
            SelectedItemChanged?.Invoke(this, id);
        }
        #endregion
    }
}
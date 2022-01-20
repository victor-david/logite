using Restless.Logite.Database.Tables;
using Restless.Toolkit.Controls;
using Restless.Toolkit.Core.Database.SQLite;
using Restless.Toolkit.Mvvm;
using System;
using System.Data;
using System.Windows.Data;

namespace Restless.Logite.ViewModel.Domain
{
    public abstract class DomainController<T> : DataGridViewModel<T> where T:TableBase
    {
        /// <summary>
        /// Gets the domain object for this domain controller.
        /// </summary>
        public DomainRow Domain
        {
            get;
        }

        public DomainController(DomainRow domain)
        {
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
        }

        public event EventHandler<long> SelectedItemChanged;

        protected void OnSelectedItemChanged(long id)
        {
            SelectedItemChanged?.Invoke(this, id);
        }
    }
}

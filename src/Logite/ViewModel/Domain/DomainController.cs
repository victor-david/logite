﻿using Restless.Logite.Database.Tables;
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
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when activated
        /// </summary>
        /// <remarks>
        /// This method sets selected item to null and refreshes.
        /// Override if you need other logic.
        /// </remarks>
        protected override void OnActivated()
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
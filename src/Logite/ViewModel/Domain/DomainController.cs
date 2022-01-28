using Restless.Logite.Core;
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using Restless.Toolkit.Controls;
using System;
using System.Windows.Data;

namespace Restless.Logite.ViewModel.Domain
{
    public abstract class DomainController<T,TR> : ApplicationViewModel where T:RawTable<TR> where TR:RawRow
    {
        #region Private
        private object selectedItem;
        #endregion

        /************************************************************************/

        #region Properties / Events
        /// <summary>
        /// Gets the domain object for this domain controller.
        /// </summary>
        public DomainRow Domain
        {
            get;
        }

        /// <summary>
        /// Gets the table object associated with this instance
        /// </summary>
        public T Table
        {
            get => DatabaseController.Instance.GetTable<T>();
        }

        /// <summary>
        /// Gets the collection of columns. The view binds to this property so that columns can be manipulated from the VM
        /// </summary>
        public DataGridColumnCollection Columns
        {
            get;
        }

        /// <summary>
        /// Gets the list view. The UI binds to this property
        /// </summary>
        public ListCollectionView ListView
        {
            get;
        }

        /// <summary>
        /// Gets or sets the selected item of the DataGrid.
        /// </summary>
        public object SelectedItem
        {
            get => selectedItem;
            set
            {
                if (SetProperty(ref selectedItem, value))
                {
                    OnPropertyChanged(nameof(IsItemSelected));
                    OnPropertyChanged(nameof(SelectedRawRow));
                    OnSelectedItemChanged();
                }
            }
        }

        /// <summary>
        /// Gets the selected raw row
        /// </summary>
        public TR SelectedRawRow
        {
            get => selectedItem as TR;
        }

        /// <summary>
        /// Gets a boolean value that indicates if an item is selected,
        /// </summary>
        public bool IsItemSelected
        {
            get => SelectedItem != null;
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
            ListView = new ListCollectionView(Table.RawRows);
            using (ListView.DeferRefresh())
            {
                ListView.CustomSort = new GenericComparer<TR>((x, y) => OnDataRowCompare(x, y));
                ListView.Filter = (item) => item is TR raw && OnDataRowFilter(raw);
            }

            Columns = new DataGridColumnCollection();
            Commands.Add("ClearSelection", p => SelectedItem = null);
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        protected override void OnDeactivated()
        {
            SelectedItem = null;
        }

        /// <summary>
        /// Override in a derived class to filter <see cref="ListView"/>. The base implementation returns true.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>true if <paramref name="item"/> is included; otherwise, false.</returns>
        protected virtual bool OnDataRowFilter(TR item)
        {
            return true;
        }

        /// <summary>
        /// Override in a derived class to compares two specified <see cref="RawRow"/> objects.
        /// The base method returns zero.
        /// </summary>
        /// <param name="item1">The first  row</param>
        /// <param name="item2">The second data row</param>
        /// <returns>An integer value 0, 1, or -1</returns>
        protected virtual int OnDataRowCompare(TR item1, TR item2)
        {
            return 0;
        }

        /// <summary>
        /// Override in a derived class to perform actions when the <see cref="SelectedItem"/> property changes.
        /// The base implementation does nothing.
        /// </summary>
        protected virtual void OnSelectedItemChanged()
        {
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
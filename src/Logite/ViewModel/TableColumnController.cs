using Restless.Toolkit.Controls;
using System.Collections.ObjectModel;
using System.Data;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Represents a controller that displays the columns of a table.
    /// </summary>
    public class TableColumnController : TableControllerBase
    {
        #region Public properties
        /// <summary>
        /// Gets the list of data columns
        /// </summary>
        public ObservableCollection<DataColumn> DataColumns
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumnController"/> class.
        /// </summary>
        /// <param name="owner">The owner</param>
        public TableColumnController(TableViewModel owner) : base(owner)
        {
            DataColumns = new ObservableCollection<DataColumn>();
            Columns.Create("Name", "ColumnName");
            Columns.Create("Type", "DataType");
            Columns.Create("Expression", "Expression").MakeFlexWidth(2.5);
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called by the <see cref="Restless.Tools.Mvvm.ViewModelBase"/> when an update is requested.
        /// </summary>
        protected override void OnUpdate()
        {
            var table = GetOwnerSelectedTable();
            if (table != null)
            {
                DataColumns.Clear();
                foreach (DataColumn col in table.Columns)
                {
                    DataColumns.Add(col);
                }
            }
        }
        #endregion
    }
}
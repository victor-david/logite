using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using System;
using System.Data;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Represents a the base class for table information controllers.
    /// </summary>
    public abstract class TableControllerBase : DataGridViewModel<SchemaTable>
    {
        /// <summary>
        /// Gets the table view model owner.
        /// </summary>
        protected new TableViewModel Owner
        {
            get;
        }

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TableControllerBase"/> class.
        /// </summary>
        /// <param name="owner">The owner</param>
        public TableControllerBase(TableViewModel owner) : base()
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Gets the TableBase object represented by the owner's current selection.
        /// </summary>
        /// <returns>The selected table.</returns>
        protected DataTable GetOwnerSelectedTable()
        {
            var row = Owner.SelectedDataRow;
            if (row != null)
            {
                string schema = row[SchemaTable.Defs.Columns.Namespace].ToString();
                string name = row[SchemaTable.Defs.Columns.Name].ToString();
                return DatabaseController.Instance.DataSet.Tables[name, schema];
            }

            return null;
        }
        #endregion
    }
}
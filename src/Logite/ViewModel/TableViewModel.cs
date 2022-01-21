using Restless.Logite.Core;
using Restless.Logite.Database.Tables;
using Restless.Logite.Resources;
using Restless.Toolkit.Controls;
using Restless.Toolkit.Mvvm.Collections;
using System.ComponentModel;
using System.Data;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Provides the logic that is used to display information about the application tables.
    /// </summary>
    public class TableViewModel : DataGridViewModel<SchemaTable>
    {
        #region Private
        private bool isTableSelected;
        private string selectedTableTitle;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets a boolean value that indicates if a table is selected.
        /// </summary>
        public bool IsTableSelected
        {
            get => isTableSelected;
            private set => SetProperty(ref isTableSelected, value);
        }

        /// <summary>
        /// Gets the selected table title.
        /// </summary>
        public string SelectedTableTitle
        {
            get => selectedTableTitle;
            private set => SetProperty(ref selectedTableTitle, value);
        }

        /// <summary>
        /// Gets the column controller.
        /// </summary>
        public TableColumnController ColumnData
        {
            get;
        }

        /// <summary>
        /// Gets the controller for parent relations.
        /// </summary>
        public TableParentRelationController Parents
        {
            get;
        }

        /// <summary>
        /// Gets the controller for child relations.
        /// </summary>
        public TableChildRelationController Children
        {
            get;
        }

        /// <summary>
        /// Gets the unique constraints controller.
        /// </summary>
        public TableConstraintController<UniqueConstraint> Unique
        {
            get;
        }

        /// <summary>
        /// Gets the FK constraints controller.
        /// </summary>
        public TableConstraintController<ForeignKeyConstraint> Foreign
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TableViewModel"/> class.
        /// </summary>
        public TableViewModel() : base()
        {
            DisplayName = "Tables";

            Columns.Create("Schema", SchemaTable.Defs.Columns.NamespaceDisplay).MakeFixedWidth(FixedWidth.W096);
            Columns.SetDefaultSort(Columns.Create("Name", SchemaTable.Defs.Columns.NameDisplay), ListSortDirection.Ascending);
            Columns.Create("Cols", SchemaTable.Defs.Columns.ColumnCount).MakeFixedWidth(FixedWidth.W052);
            Columns.Create("Rows", SchemaTable.Defs.Columns.RowCount).MakeFixedWidth(FixedWidth.W052);
            Columns.Create("PR", SchemaTable.Defs.Columns.ParentRelationCount).MakeFixedWidth(FixedWidth.W052);
            Columns.Create("CR", SchemaTable.Defs.Columns.ChildRelationCount).MakeFixedWidth(FixedWidth.W052);
            Columns.Create("C", SchemaTable.Defs.Columns.ConstraintCount).MakeFixedWidth(FixedWidth.W052);

            ColumnData = new TableColumnController(this);
            Parents = new TableParentRelationController(this);
            Children = new TableChildRelationController(this);
            Unique = new TableConstraintController<UniqueConstraint>(this);
            Foreign = new TableConstraintController<ForeignKeyConstraint>(this);
        }
        #endregion

        /************************************************************************/

        #region Protected Methods
        /// <summary>
        /// When activated establishes the table data
        /// </summary>
        protected override void OnActivated()
        {
            Table.LoadTableData();
            Refresh();
        }

        /// <summary>
        /// Compares two specified <see cref="DataRow"/> objects.
        /// </summary>
        /// <param name="item1">The first data row</param>
        /// <param name="item2">The second data row</param>
        /// <returns>An integer value 0, 1, or -1</returns>
        protected override int OnDataRowCompare(DataRow item1, DataRow item2)
        {
            int result = DataRowCompareString(item1, item2, SchemaTable.Defs.Columns.Namespace);
            if (result == 0)
            {
                result = DataRowCompareString(item1, item2, SchemaTable.Defs.Columns.Name);
            }
            return result;
        }

        /// <summary>
        /// Called when the selected item on the associated data grid has changed.
        /// </summary>
        protected override void OnSelectedItemChanged()
        {
            if (SelectedDataRow is DataRow row)
            {
                IsTableSelected = true;
                SelectedTableTitle = $"{Strings.HeaderTableDetail} [{row[SchemaTable.Defs.Columns.Namespace]}.{row[SchemaTable.Defs.Columns.Name]}]";

                ColumnData.Update();
                Parents.Update();
                Children.Update();
                Unique.Update();
                Foreign.Update();
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        #endregion
    }
}
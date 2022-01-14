using Restless.Logite.Core;
using Restless.Toolkit.Controls;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Represents a controller that displays tables contraints.
    /// </summary>
    /// <typeparam name="T">The type of contraint that this controller handles.</typeparam>
    public class TableConstraintController<T> : TableControllerBase where T: Constraint
    {
        #region Public properties
        /// <summary>
        /// Gets the collection of unique constraints for this controller
        /// </summary>
        public ObservableCollection<T> Constraints
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TableConstraintController{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner</param>
        public TableConstraintController(TableViewModel owner) : base(owner)
        {
            Constraints = new ObservableCollection<T>();
            Columns.Create("Name", "ConstraintName").MakeFixedWidth(FixedWidth.W136);
            Columns.Create("Table", "Table.TableName");
            Columns.Create("Column", "Columns[0].ColumnName");
            if (typeof(T) == typeof(ForeignKeyConstraint))
            {
                Columns.Create("Delete Rule", "DeleteRule");
                Columns.Create("Accept/Reject Rule", "AcceptRejectRule");
            }
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
                Constraints.Clear();
                foreach (T c in table.Constraints.OfType<T>())
                {
                    Constraints.Add(c);
                }
            }
        }
        #endregion
    }
}
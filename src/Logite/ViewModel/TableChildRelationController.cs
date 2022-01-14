using Restless.Logite.Core;
using Restless.Toolkit.Controls;
using System.Data;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Represents a controller that 
    /// </summary>
    public class TableChildRelationController : TableRelationController
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TableChildRelationController"/> class.
        /// </summary>
        /// <param name="owner">The owner</param>
        public TableChildRelationController(TableViewModel owner) : base(owner)
        {
            Columns.Create("Name", "RelationName").MakeFixedWidth(RelationWidth);
            Columns.Create("Col", "ParentColumns[0].ColumnName").MakeFixedWidth(FixedWidth.W052);
            Columns.Create("Child Table", "ChildTable.TableName");
            Columns.Create("Child Column", "ChildColumns[0].ColumnName").MakeFlexWidth(1.5);
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called by <see cref="Restless.Tools.Mvvm.ViewModelBase"/> when an update is requested.
        /// </summary>
        protected override void OnUpdate()
        {
            var table = GetOwnerSelectedTable();
            if (table != null)
            {
                Relations.Clear();
                foreach (DataRelation relation in table.ChildRelations)
                {
                    Relations.Add(relation);
                }
            }
        }
        #endregion
    }
}
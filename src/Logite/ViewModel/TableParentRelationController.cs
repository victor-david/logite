using Restless.Toolkit.Controls;
using System.Data;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Represents a controller that displays the parent relations of a table.
    /// </summary>
    public class TableParentRelationController : TableRelationController
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TableParentRelationController"/> class.
        /// </summary>
        /// <param name="owner">The owner</param>
        public TableParentRelationController(TableViewModel owner) : base(owner)
        {
            Columns.Create("Name", "RelationName").MakeFixedWidth(RelationWidth);
            Columns.Create("Parent Table", "ParentTable.TableName");
            Columns.Create("Parent Column", "ParentColumns[0].ColumnName");
            Columns.Create("Col", "ChildColumns[0].ColumnName");
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
                Relations.Clear();
                foreach (DataRelation relation in table.ParentRelations)
                {
                    Relations.Add(relation);
                }
            }
        }
        #endregion
    }
}
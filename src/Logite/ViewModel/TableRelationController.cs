using Restless.Logite.Core;
using System.Collections.ObjectModel;
using System.Data;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Represents the base class for a controller that handles the display of table relations. This class must be inherited.
    /// </summary>
    public abstract class TableRelationController : TableControllerBase
    {
        #region Protected fields
        /// <summary>
        /// Defines the column width that derived classes use to create a column that displays the relation name.
        /// </summary>
        protected const int RelationWidth = FixedWidth.W136;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the collection of relations for this controller
        /// </summary>
        public ObservableCollection<DataRelation> Relations
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TableRelationController"/> class.
        /// </summary>
        /// <param name="owner">The owner</param>
        public TableRelationController(TableViewModel owner)  : base(owner)
        {
            Relations = new ObservableCollection<DataRelation>();
        }
        #endregion
    }
}
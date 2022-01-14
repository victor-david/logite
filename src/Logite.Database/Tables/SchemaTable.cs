using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System.Data;
using System.Linq;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Represents the table that displays a list of tables in the data set. This table
    /// resides in a memory only attached database.
    /// </summary>
    public class SchemaTable : Core.ApplicationTableBase
    {
        #region Public properties
        /// <summary>
        /// Provides static definitions for table properties such as column names and relation names.
        /// </summary>
        public static class Defs
        {
            /// <summary>
            /// Specifies the name of this table.
            /// </summary>
            public const string TableName = "schema";

            /// <summary>
            /// Provides static column names for this table.
            /// </summary>
            public static class Columns
            {
                /// <summary>
                /// The name of the id column. This is the table's primary key.
                /// </summary>
                public const string Id = DefaultPrimaryKeyName;

                /// <summary>
                /// Schema. Hold the name of the table's namespace unaltered.
                /// </summary>
                public const string Namespace = "namespace";

                /// <summary>
                /// Namespace display. Hold the name of the table's namespace in upper case.
                /// </summary>
                /// <remarks>
                /// The reason there are two namespace columns is because in the UI
                /// they look better in upper case, but when locating the table
                /// in the data set by namespace and name, that's case sensitive.
                /// Could use name.ToLower() when querying the data set, but
                /// that would mean that if a table name was in mixed case,
                /// it wouldn't work.
                /// </remarks>
                public const string NamespaceDisplay = "namespacedisp";

                /// <summary>
                /// Name. Holds the name of the table unaltered.
                /// </summary>
                public const string Name = "name";

                /// <summary>
                /// Name display. Holds the name of the table in upper case.
                /// </summary>
                /// <remarks>
                /// The reason there are two name columns is the same reason
                /// that Namespace has two. See remarks on <see cref="NamespaceDisplay"/>.
                /// </remarks>
                public const string NameDisplay = "namedisp";

                /// <summary>
                /// The name of the column count column.
                /// </summary>
                public const string ColumnCount = "colcount";

                /// <summary>
                /// The name of the row count column.
                /// </summary>
                public const string RowCount = "rowcount";

                /// <summary>
                /// The name of the parent relation count column.
                /// </summary>
                public const string ParentRelationCount = "parentrelcount";

                /// <summary>
                /// The name of the child relation count column.
                /// </summary>
                public const string ChildRelationCount = "childrelcount";

                /// <summary>
                /// The name of the contraint count column.
                /// </summary>
                public const string ConstraintCount = "constraintcount";
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaTable"/> class.
        /// </summary>
        public SchemaTable() : base(DatabaseController.MemorySchemaName, Defs.TableName)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Loads the data from the database into the Data collection for this table.
        /// Satisfies the implementation of the base class, but in this case, nothing is loaded
        /// because this table resides in a memory only attached database. Each time the app is started, 
        /// the table is empty.
        /// </summary>
        public override void Load()
        {
            Load(null, Defs.Columns.Id);
        }

        /// <summary>
        /// Fills the table with data about the application's tables.
        /// </summary>
        public void LoadTableData()
        {
            Rows.Clear();
            long id = 100;
            foreach (TableBase table in Controller.DataSet.Tables.OfType<TableBase>())
            {
                if (table != this)
                {
                    DataRow row = NewRow();
                    row[Defs.Columns.Id] = id++;
                    row[Defs.Columns.Namespace] = table.Namespace;
                    row[Defs.Columns.NamespaceDisplay] = table.Namespace.ToUpper();
                    row[Defs.Columns.Name] = table.TableName;
                    row[Defs.Columns.NameDisplay] = table.TableName.ToUpper();
                    row[Defs.Columns.ColumnCount] = table.Columns.Count;
                    row[Defs.Columns.RowCount] = table.Rows.Count;
                    row[Defs.Columns.ParentRelationCount] = table.ParentRelations.Count;
                    row[Defs.Columns.ChildRelationCount] = table.ChildRelations.Count;
                    row[Defs.Columns.ConstraintCount] = table.Constraints.Count;
                    Rows.Add(row);
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Gets the column definitions for this table.
        /// </summary>
        /// <returns>A <see cref="ColumnDefinitionCollection"/>.</returns>
        protected override ColumnDefinitionCollection GetColumnDefinitions()
        {
            return new ColumnDefinitionCollection()
            {
                { Defs.Columns.Id, ColumnType.Integer, true },
                { Defs.Columns.Namespace, ColumnType.Text },
                { Defs.Columns.NamespaceDisplay, ColumnType.Text },
                { Defs.Columns.Name, ColumnType.Text },
                { Defs.Columns.NameDisplay, ColumnType.Text },
                { Defs.Columns.ColumnCount, ColumnType.Integer },
                { Defs.Columns.RowCount, ColumnType.Integer },
                { Defs.Columns.ParentRelationCount, ColumnType.Integer },
                { Defs.Columns.ChildRelationCount, ColumnType.Integer },
                { Defs.Columns.ConstraintCount, ColumnType.Integer },
            };
        }
        #endregion

        /************************************************************************/

        #region RowObject
        /// <summary>
        /// Encapsulates a single row from the <see cref="SchemaTable"/>.
        /// </summary>
        public class RowObject : RowObjectBase<SchemaTable>
        {
            #region Public properties
            /// <summary>
            /// Gets the unique id.
            /// </summary>
            public long Id
            {
                get => GetInt64(Defs.Columns.Id);
            }

            /// <summary>
            /// Gets the name of the table unaltered.
            /// </summary>
            public string Name
            {
                get => GetString(Defs.Columns.Name);
            }

            /// <summary>
            /// Gets the number of columns.
            /// </summary>
            public long ColumnCount
            {
                get => GetInt64(Defs.Columns.ColumnCount);
            }

            /// <summary>
            /// Gets the number of rows.
            /// </summary>
            public long RowCount
            {
                get => GetInt64(Defs.Columns.RowCount);
            }

            /// <summary>
            /// Gets the number of parent relations.
            /// </summary>
            public long ParentRelationCount
            {
                get => GetInt64(Defs.Columns.ParentRelationCount);
            }

            /// <summary>
            /// Gets the number of child relations.
            /// </summary>
            public long ChildRelationCount
            {
                get => GetInt64(Defs.Columns.ChildRelationCount);
            }

            /// <summary>
            /// Gets the number of constraints.
            /// </summary>
            public long ConstraintCount
            {
                get => GetInt64(Defs.Columns.ConstraintCount);
            }
            #endregion

            /************************************************************************/

            #region Constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="RowObject"/> class.
            /// </summary>
            /// <param name="row">The data row</param>
            public RowObject(DataRow row)
                : base(row)
            {
            }
            #endregion
        }
        #endregion
    }
}
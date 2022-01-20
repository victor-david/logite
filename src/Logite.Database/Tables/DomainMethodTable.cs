using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Holds usage per domain and http method
    /// </summary>
    public class DomainMethodTable : Core.ApplicationTableBase
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
            public const string TableName = "domainmethod";

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
                /// The domain id.
                /// </summary>
                public const string DomainId = "domainid";

                /// <summary>
                /// The method id
                /// </summary>
                public const string MethodId = "methodid";

                /// <summary>
                /// Usage count for domain / method
                /// </summary>
                public const string UsageCount = "usagecount";

                /// <summary>
                /// Provides static column names for columns that are calculated from other values.
                /// </summary>
                public class Calculated
                {
                    /// <summary>
                    /// Method.
                    /// </summary>
                    public const string Method = "CalcMethod";
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainMethodTable"/> class.
        /// </summary>
        public DomainMethodTable() : base(Defs.TableName)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        public override void Load()
        {
            Load(null, Defs.Columns.DomainId);
        }

        /// <summary>
        /// Updates usage
        /// </summary>
        public void UpdateUsage()
        {
            foreach (DomainRow domain in Controller.GetTable<DomainTable>().EnumerateAll())
            {
                foreach (MethodRow method in Controller.GetTable<MethodTable>().EnumerateAll())
                {
                    UpdateUsage(domain.Id, method.Id);
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
                { Defs.Columns.DomainId, ColumnType.Integer, false, false },
                { Defs.Columns.MethodId, ColumnType.Integer, false, false },
                { Defs.Columns.UsageCount, ColumnType.Integer, false, false }
            };
        }

        protected override void SetDataRelations()
        {
        }

        protected override void UseDataRelations()
        {
            CreateChildToParentColumn(Defs.Columns.Calculated.Method, MethodTable.Defs.Relations.ToDomainMethod, MethodTable.Defs.Columns.Method);
        }

        protected override void OnInitializationComplete()
        {
            UpdateUsage();
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void UpdateUsage(long domainId, long methodId)
        {
            DataRow row = GetUniqueRow(Select($"{Defs.Columns.DomainId}={domainId} AND {Defs.Columns.MethodId}={methodId}"));
            if (row == null)
            {
                row = NewRow();
                row[Defs.Columns.DomainId] = domainId;
                row[Defs.Columns.MethodId] = methodId;
                Rows.Add(row);
            }

            row[Defs.Columns.UsageCount] = Controller.GetTable<LogEntryTable>().Select($"{LogEntryTable.Defs.Columns.DomainId}={domainId} AND {LogEntryTable.Defs.Columns.MethodId}={methodId}").LongLength;
        }
        #endregion
    }
}
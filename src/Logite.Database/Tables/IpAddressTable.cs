using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Data;

namespace Restless.Logite.Database.Tables
{
    public class IpAddressTable : RawTable<IpAddressRow>
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
            public const string TableName = "ipaddress";

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
                /// The request.
                /// </summary>
                public const string IpAddress = "ipaddress";

                /// <summary>
                /// Provides static column names for columns that are calculated from other values.
                /// </summary>
                public class Calculated
                {
                    /// <summary>
                    /// Number of usages.
                    /// </summary>
                    public const string UsageCount = "CalcUsageCount";
                }
            }

            /// <summary>
            /// Provides static relation names.
            /// </summary>
            public static class Relations
            {
                /// <summary>
                /// The name of the relation that relates the <see cref="IpAddressTable"/> to the <see cref="LogEntryTable"/>.
                /// </summary>
                public const string ToLogEntry = "IpToLogEntry";
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="IpAddressTable"/> class.
        /// </summary>
        public IpAddressTable() : base(Defs.TableName)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
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
                { Defs.Columns.IpAddress, ColumnType.Text },
            };
        }

        /// <inheritdoc/>
        protected override void SetDataRelations()
        {
            CreateParentChildRelation<LogEntryTable>(Defs.Relations.ToLogEntry, Defs.Columns.Id, LogEntryTable.Defs.Columns.IpAddressId);
        }

        /// <inheritdoc/>
        protected override void UseDataRelations()
        {
            string expr = string.Format("Count(Child({0}).{1})", Defs.Relations.ToLogEntry, LogEntryTable.Defs.Columns.Id);
            CreateExpressionColumn<long>(Defs.Columns.Calculated.UsageCount, expr);
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Inserts an entry if it doesn't exist.
        /// </summary>
        /// <param name="entry">The log entry</param>
        /// <returns>The newly inserted id, or the existing id</returns>
        internal override long InsertEntryIf(LogEntry entry)
        {
            long id = SelectScalarId(Defs.Columns.IpAddress, entry.RemoteAddress);
            return id != -1 ? id : InsertScalarId(Defs.Columns.IpAddress, entry.RemoteAddress);
        }

        internal override void Load(long domainId, IdCollection ids)
        {
            string sql = 
                $"SELECT IP.{Defs.Columns.Id},{Defs.Columns.IpAddress}," +
                $"COUNT(L.{LogEntryTable.Defs.Columns.Id}) " +
                $"FROM {Defs.TableName} IP " +
                $"LEFT JOIN {LogEntryTable.Defs.TableName} L " +
                $"ON (IP.{Defs.Columns.Id}=L.{LogEntryTable.Defs.Columns.IpAddressId} AND L.{LogEntryTable.Defs.Columns.DomainId}={domainId}) " +
                $"WHERE IP.{Defs.Columns.Id} IN ({ids})" +
                $"GROUP BY IP.{Defs.Columns.Id}";

            LoadFromSql(sql, (reader) =>
            {
                return new IpAddressRow()
                {
                    Id = reader.GetInt64(0),
                    IpAddress = reader.GetString(1),
                    UsageCount = reader.GetInt64(2)
                };
            });
        }

//  SELECT
//IP.id, ipaddress,
//COUNT(L.id)
//FROM ipaddress IP
//left join logentry L on (IP.id = L.ipaddressid)

//WHERE IP.id in (-1,675,676,677,678,123,526,679,680,681,682,683,574,452,487,684,427,685,778,779,780,781)
//GROUP BY IP.id

        #endregion
    }
}

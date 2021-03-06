using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;

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
        #endregion
    }
}
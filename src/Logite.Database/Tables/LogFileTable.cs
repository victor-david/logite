using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Represents the table that holds log file name info.
    /// </summary>
    public partial class LogFileTable : Core.ApplicationTableBase
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
            public const string TableName = "logfile";

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
                /// The name of the log file.
                /// </summary>
                public const string FileName = "filename";

                /// <summary>
                /// The number of lines in the file.
                /// </summary>
                public const string LineCount = "linecount";

                /// <summary>
                /// Date / time record created.
                /// </summary>
                public const string Created = "created";

                /// <summary>
                /// Date / time record updated.
                /// </summary>
                public const string Updated = "updated";
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LogFileTable"/> class.
        /// </summary>
        public LogFileTable() : base(Defs.TableName)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Loads the data from the database into the Data collection for this table.
        /// </summary>
        public override void Load()
        {
            Load(null, Defs.Columns.Created);
        }

        /// <summary>
        /// Creates a new log file record.
        /// </summary>
        /// <param name="fileName">The from name</param>
        /// <param name="lineCount">The line count</param>
        /// <returns>The newly added <see cref="LogFileRow"/></returns>
        public LogFileRow Create(string fileName, long lineCount)
        {
            var obj = new LogFileRow(NewRow());
            obj.Row[Defs.Columns.FileName] = fileName;
            obj.Row[Defs.Columns.LineCount] = lineCount;
            obj.Row[Defs.Columns.Created] = DateTime.UtcNow;
            obj.Row[Defs.Columns.Updated] = DateTime.UtcNow;
            Rows.Add(obj.Row);
            Save();
            return obj;
        }

        /// <summary>
        /// Gets a single record identified by <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A row object, or null if none.</returns>
        public LogFileRow GetSingleRecord(long id)
        {
            DataRow[] rows = Select($"{Defs.Columns.Id}={id}");
            if (rows.Length == 1)
            {
                return new LogFileRow(rows[0]);
            }
            return null;
        }
        #endregion

        /************************************************************************/

        #region Public methods (enumeration)
        public IEnumerable<LogFileRow> EnumerateAll()
        {
            foreach (var row in EnumerateRows(null, Defs.Columns.FileName))
            {
                yield return new LogFileRow(row);
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
                { Defs.Columns.FileName, ColumnType.Text, false, false},
                { Defs.Columns.LineCount, ColumnType.Integer, false, false, 0L, IndexType.Index },
                { Defs.Columns.Created, ColumnType.Timestamp },
                { Defs.Columns.Updated, ColumnType.Timestamp },
            };
        }

        /// <inheritdoc/>
        protected override void SetDataRelations()
        {
        }

        protected override void UseDataRelations()
        {
            //CreateChildToParentColumn(Defs.Columns.Calculated.FromDomainName, DomainTable.Defs.Relations.ToAliasFrom, DomainTable.Defs.Columns.Name);
            //CreateChildToParentColumn(Defs.Columns.Calculated.ToDomainName, DomainTable.Defs.Relations.ToAliasTo, DomainTable.Defs.Columns.Name);

            //string expr1 = $"{Defs.Columns.FromName} + '@' + {Defs.Columns.Calculated.FromDomainName}";
            //CreateExpressionColumn<string>(Defs.Columns.Calculated.FromEmailAddress, expr1);

            //string expr2 = $"{Defs.Columns.ToName} + '@' + {Defs.Columns.Calculated.ToDomainName}";
            //CreateExpressionColumn<string>(Defs.Columns.Calculated.ToEmailAddress, expr2);
        }
        #endregion
    }
}
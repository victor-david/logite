using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Data;
using Columns = Restless.Logite.Database.Tables.LogFileTable.Defs.Columns;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Encapsulates a single row from the <see cref="LogFileTable"/>.
    /// </summary>
    public class LogFileRow : RowObjectBase<LogFileTable>
    {
        #region Public properties
        /// <summary>
        /// Gets the id for this row object.
        /// </summary>
        public long Id
        {
            get => GetInt64(Columns.Id);
        }

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName
        {
            get => GetString(Columns.FileName);
        }

        /// <summary>
        /// Gets the line count.
        /// </summary>
        public long LineCount
        {
            get => GetInt64(Columns.LineCount);
        }

        /// <summary>
        /// Gets the date / time record created.
        /// </summary>
        public DateTime Created
        {
            get => GetDateTime(Columns.Created);
        }

        /// <summary>
        /// Gets the date / time record updated.
        /// </summary>
        public DateTime Updated
        {
            get => GetDateTime(Columns.Updated);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LogFileRow"/> class.
        /// </summary>
        /// <param name="row">The data row</param>
        public LogFileRow(DataRow row) : base(row)
        {
        }

        /// <summary>
        /// If <paramref name="row"/> is not null, creates a new instance of the <see cref="AliasRow"/> class.
        /// </summary>
        /// <param name="row">The data row, or null.</param>
        /// <returns>A new AliasRow (if <paramref name="row"/> is not null); otherwise, null.</returns>
        public static LogFileRow Create(DataRow row)
        {
            return row != null ? new LogFileRow(row) : null;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Sets <see cref="LineCount"/> to the specified value;
        /// </summary>
        /// <param name="lineCount">The line count to set</param>
        public void SetLineCount(long lineCount)
        {
            Row[Columns.LineCount] = lineCount;
        }

        /// <summary>
        /// Gets the string representation of this object.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return FileName;
        }

        /// <inheritdoc/>
        protected override void OnSetValue(string columnName, object value)
        {
            if (columnName != Columns.Updated)
            {
                Row[Columns.Updated] = DateTime.UtcNow;
            }
        }
        #endregion
    }
}
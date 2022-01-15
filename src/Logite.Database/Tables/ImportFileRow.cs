using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Data;
using Columns = Restless.Logite.Database.Tables.ImportFileTable.Defs.Columns;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Encapsulates a single row from the <see cref="ImportFileTable"/>.
    /// </summary>
    public class ImportFileRow : RowObjectBase<ImportFileTable>
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
        /// Gets the domain id.
        /// </summary>
        public long DomainId
        {
            get => GetInt64(Columns.DomainId);
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

        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportFileRow"/> class.
        /// </summary>
        /// <param name="row">The data row</param>
        public ImportFileRow(DataRow row) : base(row)
        {
        }

        /// <summary>
        /// If <paramref name="row"/> is not null, creates a new instance of the <see cref="ImportFileRow"/> class.
        /// </summary>
        /// <param name="row">The data row, or null.</param>
        /// <returns>A new AliasRow (if <paramref name="row"/> is not null); otherwise, null.</returns>
        public static ImportFileRow Create(DataRow row)
        {
            return row != null ? new ImportFileRow(row) : null;
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
        }
        #endregion
    }
}
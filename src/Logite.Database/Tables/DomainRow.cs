using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Data;
using Columns = Restless.Logite.Database.Tables.DomainTable.Defs.Columns;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Encapsulates a single row from the <see cref="DomainTable"/>.
    /// </summary>
    public class DomainRow : RowObjectBase<DomainTable>
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
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName
        {
            get => GetString(Columns.DisplayName);
            set => SetValue(Columns.DisplayName, value);
        }

        /// <summary>
        /// Gets or sets the preface.
        /// </summary>
        public string Preface
        {
            get => GetString(Columns.Preface);
            set => SetValue(Columns.Preface, value);
        }

        /// <summary>
        /// Gets or sets the display mode.
        /// </summary>
        public long DisplayMode
        {
            get => GetInt64(Columns.DisplayMode);
            set => SetValue(Columns.DisplayMode, value);
        }

        /// <summary>
        /// Gets or sets the number of past days for reporting on this domain
        /// </summary>
        public long Period
        {
            get => GetInt64(Columns.Period);
            set => SetValue(Columns.Period, value);
        }

        /// <summary>
        /// Gets or sets the bit-mapped value that represents the status codes a domain wants to use for its status chart.
        /// </summary>
        public long ChartStatus
        {
            get => GetInt64(Columns.ChartStatus);
            set => SetValue(Columns.ChartStatus, value);
        }

        /// <summary>
        /// Gets the count of log entries for this domain.
        /// </summary>
        public long LogEntryCount
        {
            get => GetInt64(Columns.LogEntryCount);
            internal set => SetValue(Columns.LogEntryCount, value);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainRow"/> class.
        /// </summary>
        /// <param name="row">The data row</param>
        public DomainRow(DataRow row) : base(row)
        {
        }

        /// <summary>
        /// If <paramref name="row"/> is not null, creates a new instance of the <see cref="DomainRow"/> class.
        /// </summary>
        /// <param name="row">The data row, or null.</param>
        /// <returns>A new DomainRow (if <paramref name="row"/> is not null); otherwise, null.</returns>
        public static DomainRow Create(DataRow row)
        {
            return row != null ? new DomainRow(row) : null;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets the string representation of this object.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return DisplayName;
        }
        #endregion
    }
}
using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Columns = Restless.Logite.Database.Tables.IpAddressTable.Defs.Columns;

namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Encapsulates a single row from the <see cref="IpAddressTable"/>.
    /// </summary>
    public class IpAddressRow : RawRow
    {
        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the id for this row object.
        /// </summary>
        public long Id
        {
            get => GetInt64(Columns.Id);
        }

        /// <summary>
        /// Gets the ip address
        /// </summary>
        public string IpAddress
        {
            get => GetString(Columns.IpAddress);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="IpAddressRow"/> class.
        /// </summary>
        internal IpAddressRow() : base (IpAddressTable.Defs.TableName)
        {
        }
        #endregion

        /************************************************************************/

        #region Private methods
        #endregion
    }
}
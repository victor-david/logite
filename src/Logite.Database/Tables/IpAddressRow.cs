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
            get;
            internal set;
        }

        /// <summary>
        /// Gets the ip address
        /// </summary>
        public string IpAddress
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the usage count
        /// </summary>
        public long UsageCount
        {
            get;
            internal set;
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
        #region Public methods
        /// <summary>
        /// Gets the string representation of this object.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return $"Id:{Id} Ip Address:{IpAddress}";
        }
        #endregion

    }
}
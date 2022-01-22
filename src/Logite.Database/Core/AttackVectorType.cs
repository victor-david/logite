using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Provides an enumeration of attack vector types
    /// </summary>
    public enum AttackVectorType
    {
        None = 0,
        /// <summary>
        /// Attack is in the request
        /// </summary>
        Request = 1,
        /// <summary>
        /// Attack is in the referer
        /// </summary>
        Referer = 2,
        /// <summary>
        /// Attack is in the user agent
        /// </summary>
        UserAgent = 3
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Provides static values for navigation groups.
    /// </summary>
    public static class NavigationGroup
    {
        /* Values must be sequential; they represent indices into collections.*/
        /// <summary>
        /// Opening services group, summary, online center, etc.
        /// </summary>
        public const int Services = 0;

        /// <summary>
        /// Domain group
        /// </summary>
        public const int Domain = 1;

        /// <summary>
        /// Tools group, settings and various other utilities.
        /// </summary>
        public const int Tools = 2;
    }
}
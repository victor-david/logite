using System;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Represents bit mapped values that indicate which status codes
    /// a domain is interested in on a status chart
    /// </summary>
    [Flags]
    public enum ChartStatus
    {
        /// <summary>
        /// Domain wants to see 200 codes in chart.
        /// </summary>
        Code200 = 1,
        /// <summary>
        /// Domain wants to see 302 codes in chart.
        /// </summary>
        Code302 = 32,
        /// <summary>
        /// Domain wants to see 304 codes in chart.
        /// </summary>
        Code304 = 64,
        /// <summary>
        /// Domain wants to see 400 codes in chart.
        /// </summary>
        Code400 = 512,
        /// <summary>
        /// Domain wants to see 404 codes in chart.
        /// </summary>
        Code404 = 1024,
        /// <summary>
        /// Domain wants to see 444 codes in chart.
        /// </summary>
        Code444 = 2048,
        /// <summary>
        /// Domain wants to see 500 codes in chart.
        /// </summary>
        Code500 = 16384
    }
}
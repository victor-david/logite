namespace Restless.Logite.Core
{
    /// <summary>
    /// Provides an enumeration of time filters that may be applied to a register or other list of items.
    /// </summary>
    public enum TimeFilter
    {
        /// <summary>
        /// All.
        /// </summary>
        All = 0,
        /// <summary>
        /// Within the last 3 days
        /// </summary>
        Days003 = 3,
        /// <summary>
        /// Within the last 7 days.
        /// </summary>
        Days007 = 7,
        /// <summary>
        /// Within the last 14 days.
        /// </summary>
        Days014 = 14,
        /// <summary>
        /// Within the last 30 days.
        /// </summary>
        Days030 = 30,
        /// <summary>
        /// Within the last 60 days.
        /// </summary>
        Days060 = 60,
        /// <summary>
        /// Within the last 90 days.
        /// </summary>
        Days090 = 90,
        /// <summary>
        /// Within the last 120 days.
        /// </summary>
        Days120 = 120,
        /// <summary>
        /// Within the last 180 days.
        /// </summary>
        Days180 = 180,
        /// <summary>
        /// Within the last year.
        /// </summary>
        Years1 = 365,
        /// <summary>
        /// Within the last two years.
        /// </summary>
        Years2 = 365 * 2,
    }
}
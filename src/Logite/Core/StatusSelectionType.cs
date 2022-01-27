namespace Restless.Logite.Core
{
    /// <summary>
    /// Provides an enumeration of status selection types
    /// </summary>
    public enum StatusSelectionType
    {
        /// <summary>
        /// The status selection is used to select a status value.
        /// </summary>
        Status = 100,
        /// <summary>
        /// The status selection represents a separator.
        /// </summary>
        Separator = 500,
        /// <summary>
        /// The status selection is used to clear selected status.
        /// </summary>
        Clear = 1000,
    }
}

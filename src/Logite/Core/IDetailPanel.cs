using System.Windows;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Describes properties that a panel that uses detail must implement.
    /// </summary>
    public interface IDetailPanel
    {
        /// <summary>
        /// Gets or sets a value that determines if detail is visible.
        /// </summary>
        bool IsDetailVisible { get; set; }

        /// <summary>
        /// Gets the minimum width for detail panel.
        /// </summary>
        double DetailMinWidth { get; }

        /// <summary>
        /// Gets the maximum width for detail panel.
        /// </summary>
        double DetailMaxWidth { get; }

        /// <summary>
        /// Gets or sets the width of the detail panel.
        /// </summary>
        GridLength DetailWidth { get; set; }
    }
}
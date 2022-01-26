using Restless.Toolkit.Mvvm;
using System;
using System.Windows.Media;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Represents a single item for a status selector
    /// </summary>
    public class StatusSelection : ObservableObject
    {
        #region Private
        private bool isSelected;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the name of this selection.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Gets the value of this selection, 200, 404, etc.
        /// </summary>
        public long Value
        {
            get;
        }

        /// <summary>
        /// Gets the bit-mapped value of this selection, 1,2,4,8,16, etc.
        /// </summary>
        public long BitValue
        {
            get;
        }

        /// <summary>
        /// Gets the type of status selection
        /// </summary>
        public StatusSelectionType Type
        {
            get;
        }

        /// <summary>
        /// Gets the solid color brush used for the status
        /// </summary>
        public Brush StatusBrush
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether this item is selected.
        /// </summary>
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusSelection"/> class.
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="type">The type of status selection</param>
        /// <param name="value">The standard value for this selector, 200, 404, etc.</param>
        /// <param name="bitValue">The bit mapped value that identifies the status</param>
        /// <param name="brush">The brush used for this selection</param>
        public StatusSelection(string name, StatusSelectionType type, long value, long bitValue, Brush brush)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            Name = name;
            Type = type;
            Value = value;
            BitValue = bitValue;
            StatusBrush = brush;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets a string representation of this object.
        /// </summary>
        /// <returns>A string that describes the object.</returns>
        public override string ToString()
        {
            return $"Name:{Name} Value:{Value} BitValue:{BitValue} Selected:{IsSelected}";
        }
        #endregion
    }
}
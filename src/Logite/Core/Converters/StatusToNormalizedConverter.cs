using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Restless.Logite.Core.Converters
{
    /// <summary>
    /// Provides a converter that accepts a long value for an http status and returns its normalized group value
    /// </summary>
    /// <remarks>
    /// Converts values from a range to a base value. For instance, a value of 400-499 returns 400.
    /// </remarks>
    public class StatusToNormalizedConverter : MarkupExtension, IValueConverter
    {
        #region Constructor
        public StatusToNormalizedConverter()
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Converts a long status value to a normalized value of the group
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used</param>
        /// <param name="culture">Not used.</param>
        /// <returns>A long value</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        { 
            if (value is long l)
            {
                if (l >= 200 && l < 300) return 200L;
                if (l >= 300 && l < 400) return 300L;
                if (l >= 400 && l < 500) return 400L;
                if (l >= 500 && l < 600) return 500L;
                return l;
            }
            return value;
        }

        /// <summary>
        /// This method is not used. It throws a <see cref="NotImplementedException"/>
        /// </summary>
        /// <param name="value">n/a</param>
        /// <param name="targetType">n/a</param>
        /// <param name="parameter">n/a</param>
        /// <param name="culture">n/a</param>
        /// <returns>n/a</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the object that is set as the value of the target property for this markup extension. 
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>This object.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        #endregion
    }
}
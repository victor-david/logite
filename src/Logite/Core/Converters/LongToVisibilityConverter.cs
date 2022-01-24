using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Restless.Logite.Core.Converters
{
    /// <summary>
    /// Provides a converter that converts a long value to Visibility.Visible or Visibility.Collapsed 
    /// dpending on whether it matches the converter parameter.
    /// </summary>
    public class LongToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Converts a long value to a Visibility
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Used to compare to <paramref name="value"/></param>
        /// <param name="culture">Not used.</param>
        /// <returns>A Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long v && parameter is long p)
            {
                return v == p ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
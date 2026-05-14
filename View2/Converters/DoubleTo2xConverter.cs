using System.Globalization;
using System.Windows.Data;

namespace View2.Converters
{
    /// <summary>
    /// Converts a double value to its double (multiplies by 2).
    /// Used to convert ball radius to diameter for rendering.
    /// </summary>
    public class DoubleTo2xConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return d * 2.0;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

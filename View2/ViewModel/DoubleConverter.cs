using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace View2.ViewModel
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (value is double d)
            {
                return d.ToString(culture);
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (value as string) ?? string.Empty;
            s = s.Trim();
            if (string.IsNullOrEmpty(s)) return 0.0;

            // Try with current culture first
            if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, culture, out var d))
                return d;

            // If user typed dot but current culture expects comma, try invariant
            if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out d))
                return d;

            // Replace dot with current decimal separator and try again
            var sep = culture.NumberFormat.NumberDecimalSeparator;
            var alt = s.Replace(".", sep).Replace(",", sep);
            if (double.TryParse(alt, NumberStyles.Float | NumberStyles.AllowThousands, culture, out d))
                return d;

            // If parsing fails, do not update source
            return DependencyProperty.UnsetValue;
        }
    }
}

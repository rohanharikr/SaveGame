using System.Globalization;
using System.Windows.Data;

namespace SaveGame.Converters
{
    class RatingCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (value is double v && v == 0.0))
                return "N/A";
            else
                return (int)(double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using IGDB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaveGame.Converters
{
    class EpochToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is DateTimeOffset))
                return "N/A";

            DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
            string formattedDate = dateTimeOffset.ToString("MMMM d, yyyy");
            return formattedDate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

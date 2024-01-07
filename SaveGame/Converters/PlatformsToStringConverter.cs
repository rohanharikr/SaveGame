using IGDB;
using IGDB.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaveGame.Converters
{
    class PlatformsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder platforms = new StringBuilder();
            if(value is Platform[] platformList)
            {
                for (int i=0; i < platformList.Length; i++)
                {
                    platforms.Append(platformList[i].Name);
                    
                    if(i != (platformList.Length - 1))
                        platforms.Append(", ");
                }
            }
            if (platforms.Length == 0)
                return "N/A";

            return platforms.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

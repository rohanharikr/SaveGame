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
    class LanguagesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder languages = new StringBuilder();
            if(value is LanguageSupport[] languageList)
            {
                for (int i=0; i < languageList.Length; i++)
                {
                    languages.Append(languageList[i].Language.Value.Name);
                    
                    if(i != (languageList.Length - 1))
                        languages.Append(", ");
                }
            }
            if (languages.Length == 0)
                return "N/A";

            return languages.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

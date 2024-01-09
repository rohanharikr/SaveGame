using IGDB.Models;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace SaveGame.Converters
{
    class LanguagesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder languages = new();
            if(value is LanguageSupport[] languageList)
            {
                for (int i=0; i < languageList.Length; i++)
                {
                    //languages.Append(languageList[i].Language.Value.Name);
                    
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

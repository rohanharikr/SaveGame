using IGDB.Models;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace SaveGame.Converters
{
    class GenresToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder genres = new StringBuilder();
            if(value is Genre[] genreList)
            {
                for (int i=0; i < genreList.Length; i++)
                {
                    genres.Append(genreList[i].Name);
                    
                    if(i != (genreList.Length - 1))
                        genres.Append(" • ");
                }
            }
            if (genres.Length == 0)
                return "N/A";

            return genres.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

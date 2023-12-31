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
            return genres.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

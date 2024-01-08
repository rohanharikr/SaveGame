using IGDB;
using System.Globalization;
using System.Windows.Data;

namespace SaveGame.Converters
{
    class CoverArtConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string url = IGDB.ImageHelper.GetImageUrl(imageId: (string)value, size: ImageSize.CoverBig, retina: true);
            return "https:" + url;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

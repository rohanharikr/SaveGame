using IGDB;
using System.Globalization;
using System.Windows.Data;

namespace SaveGame.Converters
{
    class ScreenshotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string url = ImageHelper.GetImageUrl(imageId: (string)value, size: ImageSize.ScreenshotBig, retina: true);
            return "https:" + url;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

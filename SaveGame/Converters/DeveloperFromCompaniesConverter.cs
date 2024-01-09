using IGDB.Models;
using System.Globalization;
using System.Windows.Data;

namespace SaveGame.Converters
{
    class DeveloperFromCompaniesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {;
            IList<InvolvedCompany> companies = (IList<InvolvedCompany>)value;
            InvolvedCompany developer = companies.Where(company => company.Developer ?? false).First();
            if (developer == null)
                return companies.First().Company.Name;
            else
                return developer.Company.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

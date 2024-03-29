﻿using IGDB.Models;
using System.Globalization;
using System.Windows.Data;

namespace SaveGame.Converters
{
    class DeveloperFromCompaniesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {;
            InvolvedCompany[] companies = (InvolvedCompany[])value;
            InvolvedCompany developer = companies.Where(company => company.Developer ?? false).First();
            if (developer == null)
                return companies.First().Company.Value.Name;
            else
                return developer.Company.Value.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

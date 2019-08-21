using System;
using System.Globalization;
using Informer.Models;
using Xamarin.Forms;

namespace Informer
{
    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = true;

            if (parameter != null)
            {
                Boolean.TryParse(parameter.ToString(), out b);
            }

            return b ? value == null : value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}

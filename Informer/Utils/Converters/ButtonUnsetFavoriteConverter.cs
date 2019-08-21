using System;
using System.Globalization;
using Informer.Models;
using Xamarin.Forms;

namespace Informer.Utils.Converters
{
    public class ButtonUnsetFavoriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Item)
            {
                return (value as Item).Favorite != null;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}

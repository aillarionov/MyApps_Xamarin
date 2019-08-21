using System;

namespace Xamarin.Forms
{
    //[Xaml.ProvideCompiled("Xamarin.Forms.Core.XamlC.DataTypeConverter")]
    //[Xaml.TypeConversion(typeof(Uri))]
    public class DataTypeConverter : TypeConverter
    {

        bool CanConvert(Type type)
        {
            if (type == typeof(byte[]))
                return true;

            return false;
        }
    }
}
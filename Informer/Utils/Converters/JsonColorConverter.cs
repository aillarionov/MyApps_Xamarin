using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Informer.Utils.Converters
{
    public class JsonColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Color));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((Color)value).ToHexString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            ColorTypeConverter c = new ColorTypeConverter();
            return c.ConvertFromInvariantString((String)reader.Value);
            //return Color.FromHex((String)reader.Value);
        }
    }
}
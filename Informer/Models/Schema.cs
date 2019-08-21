using System;
using Informer.Utils.Converters;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Informer.Models
{
    public class Schema
    {
        [JsonConverter(typeof(JsonColorConverter))]
        public Color TitleColor { get; set; }

        [JsonConverter(typeof(JsonColorConverter))]
        public Color TitleLineColor { get; set; }

        [JsonConverter(typeof(JsonColorConverter))]
        public Color MainBackgroundColor { get; set; }

        [JsonConverter(typeof(JsonColorConverter))]
        public Color MainForegroundColor { get; set; }

        [JsonConverter(typeof(JsonColorConverter))]
        public Color DividerColor { get; set; }

        [JsonConverter(typeof(JsonColorConverter))]
        public Color TextColor { get; set; }

        [JsonConverter(typeof(JsonColorConverter))]
        public Color ButtonBorderColor { get; set; }

        [JsonConverter(typeof(JsonColorConverter))]
        public Color ButtonBackgroundColor { get; set; }
    }
}

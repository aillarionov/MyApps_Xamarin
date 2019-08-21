using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Informer.Utils
{
    [ContentProperty("Source")]
    public class LocalImageResource : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }
            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource("Resources.Images." + Source);

            return imageSource;
        }
    }
}
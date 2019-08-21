using System;
using Android.Content.Res;
using Informer.Utils;

namespace Informer.Droid
{
    public class AndroidDisplaySettings : IDisplaySettings
    {
        public int GetHeight()
        {
            return Resources.System.DisplayMetrics.HeightPixels;
        }

        public int GetWidth()
        {
            return Resources.System.DisplayMetrics.WidthPixels;
        }
    }
}

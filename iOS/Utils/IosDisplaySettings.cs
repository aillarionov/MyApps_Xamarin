using System;
using Informer.Utils;
using UIKit;

namespace Informer.iOS
{
    public class IosDisplaySettings : IDisplaySettings
    {
        public int GetHeight()
        {
            return (int)UIScreen.MainScreen.NativeBounds.Height;
        }

        public int GetWidth()
        {
            return (int)UIScreen.MainScreen.NativeBounds.Width;
        }
    }
}

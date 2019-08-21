using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using System.Threading.Tasks;
using Informer.Controls;
using AdSupport;

[assembly: Dependency(typeof(Informer.iOS.IOSAD))]
namespace Informer.iOS
{
    public class IOSAD : IAD
    {
        public string GetAdId()
        {
            if (ASIdentifierManager.SharedManager.IsAdvertisingTrackingEnabled)
            {
                return ASIdentifierManager.SharedManager.AdvertisingIdentifier.AsString();
            }
            else 
            {
                return null;
            }
        }
    }
}
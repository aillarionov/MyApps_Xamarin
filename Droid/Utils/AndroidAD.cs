using Android.Content;
using Xamarin.Forms;
using System.Threading.Tasks;
using Informer.Utils;
using Informer.Controls;
using Android.Gms.Ads.Identifier;
using Android.Gms.Common;
using System;
using Java.IO;

[assembly: Dependency(typeof(Informer.Droid.AndroidAD))]
namespace Informer.Droid
{
    public class AndroidAD : IAD
    {
        public string GetAdId()
        {
            if (int.Parse(Android.OS.Build.VERSION.Sdk) < 19) // kitkat
            {
                return null;
            }

            AdvertisingIdClient.Info idInfo = null;
            try
            {
                idInfo = AdvertisingIdClient.GetAdvertisingIdInfo(Android.App.Application.Context);
            }
            catch (GooglePlayServicesNotAvailableException e)
            {
                e.PrintStackTrace();
            }
            catch (GooglePlayServicesRepairableException e)
            {
                e.PrintStackTrace();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }

            String advertId = null;
            try
            {
                advertId = idInfo.Id;
            }
            catch (Java.Lang.NullPointerException e)
            {
                e.PrintStackTrace();
            }

            return advertId;
        }
    }
}
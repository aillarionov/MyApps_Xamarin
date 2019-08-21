using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using System.Threading.Tasks;
using Informer.Controls;

[assembly: Dependency(typeof(Informer.iOS.IOSPhone))]
namespace Informer.iOS
{
    public class IOSPhone : IPhone
    {
        public Task Call(string phoneNumber)
        {
            var nsurl = new NSUrl(new Uri($"telprompt:{phoneNumber}").AbsoluteUri);
            if (!string.IsNullOrWhiteSpace(phoneNumber) &&
                    UIApplication.SharedApplication.CanOpenUrl(nsurl))
            {
                //UIApplication.SharedApplication.OpenUrl(nsurl);
                //UIApplication.SharedApplication.Open(nsurl);
                UIApplicationOpenUrlOptions option = new UIApplicationOpenUrlOptions();
                option.OpenInPlace = true;
                UIApplication.SharedApplication.OpenUrl(nsurl, option, null);
            }
            return Task.FromResult(true);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Plugin.PushNotification;
using UIKit;
using UserNotifications;

namespace Informer.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsMaps.Init ();


            var APP = new App(new IosDisplaySettings(), new IOSAD());
            LoadApplication(APP);

            PushNotificationManager.Initialize(launchOptions);

            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            PushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            PushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            PushNotificationManager.DidReceiveMessage(userInfo);
        }

    }


}

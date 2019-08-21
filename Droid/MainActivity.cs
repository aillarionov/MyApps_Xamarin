using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Firebase;
using Plugin.PushNotification;
using Xamarin.Forms.Platform.Android;

namespace Informer.Droid
{
    [Activity(Label = "MyApps", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // detect bug at compile time (Firebase not initialized)
            var dsid = GetString(Resource.String.gcm_defaultSenderId);



            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //base.SetTheme(Resource.Style.MyTheme);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.FormsMaps.Init (this, bundle);

            var APP = new App(new AndroidDisplaySettings(), new AndroidAD());

            APP.OnThemeChange += APP_OnThemeChange;

            LoadApplication(APP);

            //If debug you should reset the token each time.
            #if DEBUG
            PushNotificationManager.Initialize(this, true);
            #else
            PushNotificationManager.Initialize(this, false);
            #endif

            PushNotificationManager.ProcessIntent(Intent);

            CrossPushNotification.Current.OnNotificationReceived += DoNotify;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            PushNotificationManager.ProcessIntent(intent);
        }

        void APP_OnThemeChange(object sender, Utils.SchemaEventArgs e)
        {
            TitleColor = e.Schema.TitleColor.ToAndroid();  


            //SetColor(Resource.Attribute.colorPrimary, e.Schema.Primary);
            //SetColor(Resource.Attribute.colorPrimaryDark, e.Schema.PrimaryDark);
            //SetColor(Resource.Attribute.colorAccent, e.Schema.Primary);

        }

        private void SetColor(int param, Xamarin.Forms.Color value)
        {
            Android.Util.TypedValue a = new Android.Util.TypedValue();
            Theme.ResolveAttribute(param, a, true);
            var color = Application.Context.GetDrawable(a.ResourceId);
            ((Android.Graphics.Drawables.ColorDrawable)color).Color = value.ToAndroid();
        }


        void DoNotify(object source, Plugin.PushNotification.Abstractions.PushNotificationDataEventArgs e)
        {
            /*
            if (e.Data.TryGetValue("silent", out object silent) && (silent.ToString() == "true" || silent.ToString() == "1"))
                return;

            string title = Application.Context.ApplicationInfo.LoadLabel(Application.Context.PackageManager);
            string text = e.Data.ContainsKey("body") ? e.Data["body"].ToString() : "";

            int id = 0;
            if (e.Data.ContainsKey("id")) 
            {
                int.TryParse(e.Data["id"].ToString(), out id);    
            }

            string tag = String.Empty;
            if (e.Data.ContainsKey("tag"))
            {
                tag = e.Data["tag"].ToString();
            }

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .SetContentTitle(title)
                .SetContentText("BG: "+ text)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Alarm))
                .SetSmallIcon(PushNotificationManager.IconResource);
            NotificationManager notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            notificationManager.Notify(tag, id, builder.Build());
            */

        }
    }
}

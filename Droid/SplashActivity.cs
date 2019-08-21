using System;
using Android.App;
using Android.Support.V7.App;

namespace Informer.Droid
{
    [Activity(Label = "MyApps", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = false, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}

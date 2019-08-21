using System;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Switch), typeof(Informer.Droid.CustomSwitchRenderer))]
namespace Informer.Droid
{
    public class CustomSwitchRenderer : SwitchRenderer
    {
        public CustomSwitchRenderer(Context context) : base(context)
        {
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (Control.Checked)
                {
                    Control.ThumbDrawable.SetColorFilter(App.group.Schema.TitleColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.Multiply);
                    Control.TrackDrawable.SetColorFilter(App.group.Schema.TitleColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.Multiply);
                }
                else 
                {
                    Control.ThumbDrawable.SetColorFilter(App.group.Schema.ButtonBorderColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.Multiply);
                    Control.TrackDrawable.SetColorFilter(App.group.Schema.ButtonBorderColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.Multiply);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null)
            {
                if (Control.Checked)
                {
                    Control.ThumbDrawable.SetColorFilter(App.group.Schema.TitleColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.Multiply);
                    Control.TrackDrawable.SetColorFilter(App.group.Schema.TitleColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.Multiply);
                }
                else
                {
                    Control.ThumbDrawable.SetColorFilter(App.group.Schema.ButtonBorderColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.Multiply);
                    Control.TrackDrawable.SetColorFilter(App.group.Schema.ButtonBorderColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.Multiply);
                }
            }
        }
    }
}

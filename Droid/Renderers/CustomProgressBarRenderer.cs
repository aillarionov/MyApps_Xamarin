using System;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ProgressBar), typeof(Informer.Droid.CustomProgressBarRenderer))]
namespace Informer.Droid
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        public CustomProgressBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.ProgressDrawable.SetColorFilter(App.group.Schema.TitleColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcIn);
                //Control.ProgressTintListColor.FromRgb(182, 231, 233).ToAndroid();
                //Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(color);
                //Control.ScaleY = 10;
            }
        }
    }
}

using System;
using Android.Content;
using Android.Text;
using Android.Text.Method;
using Android.Widget;
using Informer.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HTMLLabel), typeof(Informer.Droid.HTMLLabelRenderer))]
namespace Informer.Droid
{
    public class HTMLLabelRenderer : LabelRenderer
    {
        public HTMLLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            UpdateText();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            UpdateText();
      
        }

        private void UpdateText() 
        {
            var view = (HTMLLabel)Element;
            if (view == null) return;

            ISpanned text = null;

            if (view.Text != null)
            {
                #pragma warning disable CS0618 // Type or member is obsolete
                if (int.Parse(Android.OS.Build.VERSION.Sdk) >= 24)
                {
                    text = Html.FromHtml(view.Text.ToString(), Html.FromHtmlModeLegacy);
                }
                else
                {
                    text = Html.FromHtml(view.Text.ToString());
                }
                #pragma warning restore CS0618 // Type or member is obsolete
            }

            Control.MovementMethod = LinkMovementMethod.Instance;
            Control.SetText(text, TextView.BufferType.Spannable);
        }
    }
}
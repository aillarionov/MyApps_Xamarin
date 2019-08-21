using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Content;

[assembly: ExportRenderer(typeof(Button), typeof(Informer.Droid.FlatButtonRenderer))]
namespace Informer.Droid
{
    public class FlatButtonRenderer : ButtonRenderer
    {
        public FlatButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                
                //Control.TextSize = 0.8f * (float)Element.FontSize;
            }
        }
    }
}

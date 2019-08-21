using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
       
[assembly: ExportRenderer(typeof(ListView), typeof(Informer.Droid.CustomAllListViewRendererDroid))]
namespace Informer.Droid
{
    public class CustomAllListViewRendererDroid : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Android.Widget.ListView nativeListView = Control;
                if (nativeListView == null)
                {
                    
                }
            }
        }

    }
}

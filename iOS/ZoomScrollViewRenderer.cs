using System;
using System.Linq;
using Informer.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(ZoomScrollView), typeof(Informer.iOS.ZoomScrollViewRenderer))]
namespace Informer.iOS
{
    public class ZoomScrollViewRenderer: ScrollViewRenderer
    {
        // bool zoomEnabled = false;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                ZoomScrollView scroll = (ZoomScrollView)e.NewElement;

                MaximumZoomScale = new nfloat(scroll.MaximumZoom);
                MinimumZoomScale = new nfloat(scroll.MinimumZoom);
                //SetZoomScale(new nfloat(scroll.StartZoom), false);
                ZoomScale = new nfloat(scroll.StartZoom);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (Subviews.Length > 0)
            {
                ViewForZoomingInScrollView += GetViewForZooming;
            }
            else
            {
                ViewForZoomingInScrollView -= GetViewForZooming;
            }

        }
        public UIView GetViewForZooming(UIScrollView sv)
        {
            return this.Subviews.FirstOrDefault();
        }

    }
}
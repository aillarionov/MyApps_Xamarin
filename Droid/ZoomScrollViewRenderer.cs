using System;
using Android.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Informer.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.ScaleGestureDetector;
using static Android.Widget.ImageView;

[assembly: ExportRenderer(typeof(ZoomScrollView), typeof(Informer.Droid.ZoomScrollViewRenderer))]
namespace Informer.Droid
{
    public class ZoomScrollViewRenderer : ScrollViewRenderer//, IOnScaleGestureListener
    {
        private ScaleGestureDetector _scaleDetector;
        private bool _isScaleProcess = false;
        private float _prevScale = 1f;

        public ZoomScrollViewRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var scrollView = Element as ZoomScrollView;
                //_prevScale = (float)scrollView.StartZoom;

                _scaleDetector = new ScaleGestureDetector(Context, new ClearScaleListener(
                    scale =>
                    {
                        var horScrollView = GetChildAt(0) as HorizontalScrollView;
                        var content = (horScrollView.GetChildAt(0) as ViewGroup).GetChildAt(0) as ImageRenderer;

                        var img = (content.Control as ImageView);

                        img.SetAdjustViewBounds(true);
                        img.SetScaleType(ScaleType.Center);

                        
                        
                        var xRatio = scale.FocusX / Width;
                        var yRatio = scale.FocusY / Height;

                        scrollView.AnchorX = xRatio;
                        scrollView.AnchorY = yRatio;
                    },
                    scale =>
                    {
                        _isScaleProcess = true;
                        
                        var horScrollView = GetChildAt(0) as Android.Widget.HorizontalScrollView;
                        //var content = horScrollView.GetChildAt(0);


                        var content = (horScrollView.GetChildAt(0) as ViewGroup).GetChildAt(0) as ImageRenderer;

                        var img = (content.Control as ImageView);

                        _prevScale = Math.Max((float)scrollView.MinimumZoom, Math.Min(_prevScale * scale.ScaleFactor, (float)scrollView.MaximumZoom));

                    img.ScaleX = img.ScaleY = _prevScale;

                        

                        
                    }));
            }
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            if (e.PointerCount == 2)
            {
                return _scaleDetector.OnTouchEvent(e);
            }
            else if (_isScaleProcess)
            {
                //HACK:
                //Prevent letting any touch events from moving the scroll view until all fingers are up from zooming...This prevents the jumping and skipping around after user zooms.
                if (e.Action == MotionEventActions.Up)
                    _isScaleProcess = false;
                return false;
            }
            else 
            {
                return base.OnTouchEvent(e);
            }
                
        }
    }

    public class ClearScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener
    {
        private Action<ScaleGestureDetector> _onScale;
        private Action<ScaleGestureDetector> _onScaleBegin;
        private bool _skip = false;

        public ClearScaleListener(Action<ScaleGestureDetector> onScaleBegin, Action<ScaleGestureDetector> onScale)
        {
            _onScale = onScale;
            _onScaleBegin = onScaleBegin;
        }

        public override bool OnScale(ScaleGestureDetector detector)
        {
            if (_skip)
            {
                _skip = false;
                return true;
            }
            _onScale?.Invoke(detector);
            return true;
        }

        public override bool OnScaleBegin(ScaleGestureDetector detector)
        {
            _skip = true;
            _onScaleBegin.Invoke(detector);
            return true;
        }
    }
}
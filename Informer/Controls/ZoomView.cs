using System;
using Xamarin.Forms;

namespace Informer.Controls
{
    public class ZoomView : ContentView
    {
        private const double MIN_SCALE = 1;
        private const double MAX_SCALE = 5;
        private const double OVERSHOOT = 0.1;
        private double StartScale;
        double x, y, dx, dy, sx, sy;



        public ZoomView()
        {
            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(pan);

            var pinch = new PinchGestureRecognizer();
            pinch.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinch);

            var tap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        private void OnTapped(object sender, EventArgs e)
        {
            double nfx, nfy;

            StartScale = Content.Scale;
            Content.AnchorX = Content.AnchorY = 0;

            if (Content.Scale > MIN_SCALE)
            {
                Content.ScaleTo(MIN_SCALE, 250, Easing.CubicInOut);
                //Content.TranslateTo(0, 0, 250, Easing.CubicInOut);

                nfx = sx + Content.Width * (StartScale - MIN_SCALE) * dx;
                nfy = sy + Content.Height * (StartScale - MIN_SCALE) * dy;
            }
            else
            {
                Content.ScaleTo(MAX_SCALE, 250, Easing.CubicInOut);

                nfx = sx + Content.Width * (StartScale - MAX_SCALE) * dx;
                nfy = sy + Content.Height * (StartScale - MAX_SCALE) * dy;
            }

            Translate(nfx, nfy, true);
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    Content.AnchorX = Content.AnchorY = 0;
                    x = Content.TranslationX;
                    y = Content.TranslationY;
                    break;

                case GestureStatus.Running:
                    Translate(x + e.TotalX, y + e.TotalY, false);
                    break;

                case GestureStatus.Completed:
                    x = Content.TranslationX;
                    y = Content.TranslationY;
                    break;
            }
        }

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    StartScale = Content.Scale;

                    Content.AnchorX = Content.AnchorY = 0;

                    var ndx = (Content.Width * Content.Scale) > 0 ? (this.Width * e.ScaleOrigin.X - Content.TranslationX) / (Content.Width * Content.Scale) : 0;
                    var ndy = (Content.Height * Content.Scale) > 0 ? (this.Height * e.ScaleOrigin.Y - Content.TranslationY) / (Content.Height * Content.Scale) : 0;

                    dx = Clamp(ndx, 0, 1);
                    dy = Clamp(ndy, 0, 1);

                    sx = Content.TranslationX;
                    sy = Content.TranslationY;

                    break;

                case GestureStatus.Running:
                    double current = Content.Scale + (e.Scale - 1) * StartScale;
                    Content.Scale = Clamp(current, MIN_SCALE * (1 - OVERSHOOT), MAX_SCALE * (1 + OVERSHOOT));

                    var nsx = sx + Content.Width * (StartScale - Content.Scale) * dx;
                    var nsy = sy + Content.Height * (StartScale - Content.Scale) * dy;

                    Content.TranslationX = nsx;
                    Content.TranslationY = nsy;

                    break;

                case GestureStatus.Completed:
                    double nfx, nfy;

                    if (Content.Scale > MAX_SCALE)
                    {
                        Content.ScaleTo(MAX_SCALE, 250, Easing.SpringOut);

                        nfx = sx + Content.Width * (StartScale - MAX_SCALE) * dx;
                        nfy = sy + Content.Height * (StartScale - MAX_SCALE) * dy;

                        Translate(nfx, nfy, true);
                    }
                    else if (Content.Scale < MIN_SCALE)
                    {
                        Content.ScaleTo(MIN_SCALE, 250, Easing.SpringOut);

                        nfx = sx + Content.Width * (StartScale - MIN_SCALE) * dx;
                        nfy = sy + Content.Height * (StartScale - MIN_SCALE) * dy;

                        Translate(nfx, nfy, true);
                    }
                    else 
                    {
                        nfx = sx + Content.Width * (StartScale - Content.Scale) * dx;
                        nfy = sy + Content.Height * (StartScale - Content.Scale) * dy;

                        Translate(nfx, nfy, true);

                    }

                    break;
            }
        }

        private void Translate(double nx, double ny, bool animated) 
        {
            double ntx, nty;

            if (Content.Width * Content.Scale <= this.Width)
            {
                ntx = Clamp(nx, 0, this.Width - Content.Width * Content.Scale);
            }
            else
            {
                ntx = Clamp(nx, this.Width - Content.Width * Content.Scale, 0);
            }

            if (Content.Height * Content.Scale <= this.Height)
            {
                nty = Clamp(ny, 0, this.Height - Content.Height * Content.Scale);
            }
            else
            {
                nty = Clamp(ny, this.Height - Content.Height * Content.Scale, 0);
            }

            if (animated) 
            {
                Content.TranslateTo(ntx, nty, 250, Easing.SpringOut);
            }
            else 
            {
                Content.TranslationX = ntx;
                Content.TranslationY = nty;

                //  На IOS перемещается дерганно
                //Content.TranslateTo(ntx, nty, 1, Easing.Linear);
            }
        }

        private T Clamp<T>(T value, T minimum, T maximum) where T : IComparable
        {
            if (value.CompareTo(minimum) < 0)
                return minimum;
            else if (value.CompareTo(maximum) > 0)
                return maximum;
            else
                return value;
        }
    }
}

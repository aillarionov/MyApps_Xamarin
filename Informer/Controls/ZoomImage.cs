using System;
using Xamarin.Forms;

namespace Informer.Controls
{
    public class ZoomImage : Image
    {
        private const double MIN_SCALE = 1;
        private const double MAX_SCALE = 5;
        private const double OVERSHOOT = 0.1;
        private double StartScale;
        private double LastX, LastY, StartX, StartY;


        double currentScale = 1;
        double xOffset = 0;
        double yOffset = 0;
        double x, y;


        public ZoomImage()
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

            Scale = MIN_SCALE;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            Scale = MIN_SCALE;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        private void OnTapped(object sender, EventArgs e)
        {
            Console.WriteLine(String.Format("Tapped"));


            if (Scale > MIN_SCALE)
            {
                this.ScaleTo(MIN_SCALE, 250, Easing.CubicInOut);
                this.TranslateTo(0, 0, 250, Easing.CubicInOut);
            }
            else
            {
                AnchorX = AnchorY = 0.5; //TODO tapped position
                this.ScaleTo(MAX_SCALE, 250, Easing.CubicInOut);
            }
        }
        /*
        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (Scale > MIN_SCALE)
                switch (e.StatusType)
                {
                    case GestureStatus.Started:
                        LastX = TranslationX;
                        LastY = TranslationY;
                        break;
                    case GestureStatus.Running:
                        TranslationX = Clamp(LastX + e.TotalX * Scale, -Width / 2, Width / 2);
                        TranslationY = Clamp(LastY + e.TotalY * Scale, -Height / 2, Height / 2);
                        break;
                }
        }
        */
        /*
        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    StartX = (1 - AnchorX) * Width;
                    StartY = (1 - AnchorY) * Height;
                    break;
                case GestureStatus.Running:
                    AnchorX = Clamp(1 - (StartX + e.TotalX) / Width, 0, 1);
                    AnchorY = Clamp(1 - (StartY + e.TotalY) / Height, 0, 1);
                    break;

                case GestureStatus.Completed:
                    StartX = 0;
                    StartY = 0;
                    break;
            }
        }
        */

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    x = TranslationX;
                    y = TranslationY;
                    Console.WriteLine(String.Format("Pan start"));
                    break;

                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.

                    //TranslationX = Clamp(x + e.TotalX * Scale, 0, (Parent as VisualElement).Width - Width * Scale);
                    //TranslationY = Clamp(y + e.TotalY * Scale, 0, (Parent as VisualElement).Height - Height * Scale);
                    TranslationX = x + e.TotalX * Scale;
                    TranslationY = y + e.TotalY * Scale;


                    Console.WriteLine(String.Format("TX:{0}, TY:{1}, X:{2}, Y:{3}, W:{4}, H:{5}, ID:{6}", TranslationX, TranslationY, x ,y, e.TotalX, e.TotalY, e.GestureId));
                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    x = TranslationX;
                    y = TranslationY;
                    Console.WriteLine(String.Format("Pan complete"));
                    break;
            }
        }





        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    StartScale = Scale;
                    AnchorX = e.ScaleOrigin.X;
                    AnchorY = e.ScaleOrigin.Y;
                    Console.WriteLine(String.Format("Pinch start"));
                    break;
                case GestureStatus.Running:
                    double current = Scale + (e.Scale - 1) ;
                    Scale = Clamp(current, MIN_SCALE * (1 - OVERSHOOT), MAX_SCALE * (1 + OVERSHOOT));
                    break;
                case GestureStatus.Completed:
                    if (Scale > MAX_SCALE)
                        this.ScaleTo(MAX_SCALE, 250, Easing.SpringOut);
                    else if (Scale < MIN_SCALE)
                        this.ScaleTo(MIN_SCALE, 250, Easing.SpringOut);
                    Console.WriteLine(String.Format("Pinch complete"));
                    break;
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

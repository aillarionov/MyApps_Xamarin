using System;
using CoreGraphics;
using Foundation;
using Informer.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HTMLLabel), typeof(Informer.iOS.HTMLLabelRenderer))]
namespace Informer.iOS
{
    public class HTMLLabelRenderer: ViewRenderer<Label, UITextView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            UpdateText();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Text") 
            {
                UpdateText();
            }
        }

        private void UpdateText() 
        {
            var view = (HTMLLabel)Element;
            if (view == null) return;

            if (Control == null)
            {
                var text = GenerateText(view.Text);

                UITextView textView = new UITextView(new CGRect(0, 0, view.Width, view.Height));

                if (text != null)
                {
                    textView.AttributedText = text;
                }

                textView.Font = UIFont.SystemFontOfSize((float)view.FontSize);
                textView.Editable = false;
                textView.Bounces = false;
                textView.ScrollEnabled = false;
                textView.Selectable = true;



                // Setting the data detector types mask to capture all types of link-able data
                textView.DataDetectorTypes = UIDataDetectorType.All;
                textView.BackgroundColor = UIColor.Clear;

                // overriding Xamarin Forms Label and replace with our native control
                SetNativeControl(textView);
            }
            else
            {
                var textView = Control;
                var text = GenerateText(view.Text);
                if (text != null)
                {
                    textView.AttributedText = text;
                }
                else
                {
                    textView.Text = "";
                }

                textView.Font = UIFont.SystemFontOfSize((float)view.FontSize);
                textView.Editable = false;
                textView.Bounces = false;
                textView.ScrollEnabled = false;
                textView.Selectable = true;



                // Setting the data detector types mask to capture all types of link-able data
                textView.DataDetectorTypes = UIDataDetectorType.All;
                textView.BackgroundColor = UIColor.Clear;
            }
        }

        private static NSAttributedString GenerateText(String text) 
        {
            if (text == null) 
            {
                return null;
            }

            var attr = new NSAttributedStringDocumentAttributes();
            var nsError = new NSError();
            attr.DocumentType = NSDocumentType.HTML;

            var myHtmlData = NSData.FromString(text, NSStringEncoding.Unicode);
            NSAttributedString result = new NSAttributedString(myHtmlData, attr, ref nsError);
            return result;
           
        }
    }
}

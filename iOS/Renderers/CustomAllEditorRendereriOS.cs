using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(Editor), typeof(Informer.iOS.CustomAllEditorRendereriOS))]
namespace Informer.iOS
{
    public class CustomAllEditorRendereriOS : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Layer.CornerRadius = 5;
                Control.Layer.BorderColor = App.group.Schema.MainBackgroundColor.ToCGColor();
                Control.Layer.BorderWidth = 1;
            }
        }
    }
}

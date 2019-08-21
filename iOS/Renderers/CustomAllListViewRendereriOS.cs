using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//// BUGFIX
/// Иначе при использовании HTMMLLabel в ListView падает 
[assembly: ExportRenderer(typeof(ListView), typeof(Informer.iOS.CustomAllListViewRendereriOS))]
namespace Informer.iOS
{
    public class CustomAllListViewRendereriOS : ListViewRenderer
    {
        /*
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var listView = Control as UITableView;
                listView.SeparatorInset = UIEdgeInsets.Zero;
            }
        }
        */

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {

            base.OnElementChanged(e);

            if (e.NewElement != null)
            {

                var mCollectionChanged = typeof(ListViewRenderer).GetMethod("OnCollectionChanged", BindingFlags.Instance | BindingFlags.NonPublic);

                Action<ITemplatedItemsList<Cell>> removeEvent = (items) => {
                    Delegate eventMethod = mCollectionChanged.CreateDelegate(typeof(NotifyCollectionChangedEventHandler), this);
                    items.GetType().GetEvent("CollectionChanged").RemoveEventHandler(items, eventMethod);
                };

                var templatedItems = ((ITemplatedItemsView<Cell>)e.NewElement).TemplatedItems;
                removeEvent(templatedItems);
                templatedItems.CollectionChanged += TemplatedItems_CollectionChanged;

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var templatedItems = ((ITemplatedItemsView<Cell>)Element).TemplatedItems;
                templatedItems.CollectionChanged -= TemplatedItems_CollectionChanged;
            }
            base.Dispose(disposing);
        }


        void TemplatedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Control.ReloadData();
        }
    }
}

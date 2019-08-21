using System;

using Xamarin.Forms;

namespace Informer
{
    public class MainPage : NavigationPage
    {
        public MainPage()
        {
            Page itemsPage = null;

            switch (Device.RuntimePlatform)
            {
                
                case Device.iOS:
                    itemsPage = new ItemsPage()
                    {
                        Title = "Browse"
                    };
                    itemsPage.Icon = "tab_feed.png";
                    break;
                default:
                    itemsPage = new ItemsPage()
                    {
                        Title = "Browse"
                    };
                    break;
            }
            this.PushAsync(itemsPage);
            //this.RootPage = itemsPage;
            //Children.Add(itemsPage);

            //Title = Children[0].Title;
        }
        /*
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }
        */
    }
}

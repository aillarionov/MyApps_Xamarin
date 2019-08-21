using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Informer.Utils
{
    public class MenuLink: StackLayout 
    {
        private readonly Func<Page> FuncPage;
        private Page Page;
        private String Name;

        public MenuLink(MasterDetailPage mdpage, Func<Page> funcPage, string name, FileImageSource icon)
        {
            this.Name = name;

            this.StyleClass = new List<String> { "MenuButton" };
            this.HeightRequest = 24;
            this.FuncPage = funcPage;
            this.Padding = new Thickness(10, 10);
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.Orientation = StackOrientation.Horizontal;
            this.Spacing = 10;

            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(o =>
                {
                    mdpage.Detail = new NavigationPage(getPage());
                    mdpage.IsPresented = false;
                })
            });

            this.Children.Add(new Image()
            {
                Source = icon,
                VerticalOptions = LayoutOptions.Center
            });

            this.Children.Add(new Label()
            {
                Text = this.Name,
                VerticalOptions = LayoutOptions.Center
            });

        }

        public string getName() 
        {
            return this.Name;
        }

        public Page getPage() {
            if (Page == null) {
                Page = FuncPage.Invoke();
            }

            return this.Page;
        }

    }
}

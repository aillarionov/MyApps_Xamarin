using System;
using Informer.Utils;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Informer.Models;
using Informer.Services.RPC;
using Informer.Views.Special;
using Informer.Services;
using Informer.Controls;

namespace Informer.Views
{
    public class MainMenu: MasterDetailPage
    {
        List<MenuLink> menuItems;

        public MainMenu()
        {
            StackLayout menu = new StackLayout
            {
                StyleClass = new List<String> { "Menu" },
                Padding = new Thickness(0, 0, 0, 5),
                Spacing = 0
            };

            menuItems = this.GetMenuItems();

            foreach(MenuLink menuLink in menuItems) 
            {
                menu.Children.Add(menuLink);
                menu.Children.Add(new BoxView()
                {
                    StyleClass = new List<String> { "Splitter" },
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = 1,
                    Margin = new Thickness(5, 0)
                });
            }

            StackLayout page = new StackLayout
            {
                Margin = new Thickness(0, 20, 0, 0),
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 0
            };

            StackLayout mainLogo = new StackLayout
            {
                StyleClass = new List<String> { "MainLogo" },
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 0
            };


            mainLogo.Children.Add(new Xamarin.Forms.Image()
            {
                Source = ImageSource.FromFile("menu_logo.png"),
                StyleClass = new List<String> { "Foreground" },
                Margin = new Thickness(10, 2)
            });

            mainLogo.Children.Add(new BoxView()
            {
                StyleClass = new List<String> { "MainLogoSplitter" },
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 2,
                Margin = new Thickness(0)
            });

            page.Children.Add(mainLogo);


            page.Children.Add(new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = menu,
                Padding = new Thickness(0)
            });

            this.Master = new ContentPage
            {
                Title = "Меню",
                Icon = Device.RuntimePlatform == Device.iOS ? "menu.png" : null,
                Content = page,
                Padding = new Thickness(0, 0, 0, 0)
            };

            GoToMainPage();
        }

        protected List<MenuLink> GetMenuItems() {
            List<MenuLink> items = new List<MenuLink>();

            List<Album> albums = LocalDataStore.GetAlbums(App.group.Id).Result;

            items.Add(new MenuLink(this, () => new GroupsPage(), "Выбрать выставку", new FileImageSource() { File = "exhibition.png" }));

            items.Add(new MenuLink(this, () => new AboutPage(App.group.Id), "О выставке", new FileImageSource() { File = "exhibitions.png" }));

            items.Add(new MenuLink(this, () => new SendRequestPage(App.group.Id), "Заявка на участие", new FileImageSource() { File = "entry.png" }));

            items.Add(new MenuLink(this, () => new NewsPage(App.group.Id), "Новости", new FileImageSource() { File = "news.png" }));

            items.Add(new MenuLink(this, () => new FavoritesPage(App.group.Id), "Избранное", new FileImageSource() { File = "favorites.png" }));

            items.Add(new MenuLink(this, () => new SearchPage(App.group.Id), "Поиск", new FileImageSource() { File = "search.png" }));

            items.Add(new MenuLink(this, () => new PlanPage(), "План выставки", new FileImageSource() { File = "plan.png" }));


            foreach (Album album in albums) 
            {
                if (album.Size == 0)
                {
                    continue;
                }

                switch(album.Class) {
                    case ("Item"):
                        items.Add(new MenuLink(this, () => new ItemsPage(App.group.Id, album), album.Title, new FileImageSource() { File = album.Icon }));
                        break;
                    case ("Member"):
                        items.Add(new MenuLink(this, () => new MembersPage(App.group.Id, album), album.Title, new FileImageSource() { File = album.Icon }));
                        break;
                    case ("Image"):
                        items.Add(new MenuLink(this, () => new PhotoPage(App.group.Id, album), album.Title, new FileImageSource() { File = album.Icon }));
                        break;
                }

            }

            //items.Add(new MenuLink(this, () => new GetTicketPage(App.group.Id), "Билеты", new FileImageSource() { File = "tickets.png" }));
            items.Add(new MenuLink(this, () => new MapPage(App.group.Id), "Как добраться", new FileImageSource() { File = "address.png" }));
            items.Add(new MenuLink(this, () => new SendQuestionPage(App.group.Id), "Задать вопрос", new FileImageSource() { File = "question.png" }));
            items.Add(new MenuLink(this, () => new SettingsPage(), "Выбор и настройки", new FileImageSource() { File = "setting.png" }));

            return items;
        }

        public void GoToMainPage() 
        {
            this.Detail = new NavigationPage(menuItems[1].getPage());
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.Detail is NavigationPage)
            {
                NavigationPage np = (this.Detail as NavigationPage);

                if (np.StackDepth > 1)
                {
                    return base.OnBackButtonPressed();
                }
                else
                {
                    if (np.CurrentPage != menuItems[0].getPage()) 
                    {
                        GoToMainPage();
                    }
                }
            }
                
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informer.Models;
using Informer.ViewModels;
using Informer.Views;
using Informer.Services;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Informer.Utils;
using System.Threading;

namespace Informer.Views.Special
{
    public partial class MembersPage : ContentPage
    {
        List<String> Categories = new List<String>();
        int groupId;
        Album album;

        const String AllCategories = "Все тематики (выбор)";

        public List<Member> AllItems { get; set; }
        public ObservableCollection<Member> Items { get; set; }

        private CancellationTokenSource throttleCts = new CancellationTokenSource();

        public MembersPage(int groupId, Album album)
        {
            this.groupId = groupId;
            this.album = album;

            Items = new ObservableCollection<Member>();

            InitializeComponent();

            this.Title = album.Title;

            BindingContext = this;
        }

        async Task Filter() 
        {
            await Task.Run(() =>
            {
                Items.Clear();

                String category = CategoryPicker.SelectedIndex >= 0 ? (String)CategoryPicker.ItemsSource[CategoryPicker.SelectedIndex] : null;
                String filter = String.IsNullOrEmpty(SearchQuery.Text) ? null : SearchQuery.Text.Trim().ToLower();

                if (!String.IsNullOrEmpty(category))
                {
                    foreach (Member item in AllItems)
                    {
                        foreach (String itemCategory in item.Categories)
                        {
                            if (
                                (itemCategory == category || category == AllCategories)
                                &&
                                (String.IsNullOrEmpty(filter) || (item.Raw != null && item.Raw.Contains(filter)))
                                &&
                                !Items.Contains(item)
                            )
                            {
                                Items.Add(item);
                            }
                        }
                    }
                }
            }).ConfigureAwait(false);
        }

        void Query_Change(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            Interlocked.Exchange(ref this.throttleCts, new CancellationTokenSource()).Cancel();
            Task.Delay(App.queryDelay, this.throttleCts.Token) // throttle time
                .ContinueWith(
                    delegate { this.Filter().ConfigureAwait(false); },
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

        async void Category_Changed(object sender, System.EventArgs e)
        {
            await Filter().ConfigureAwait(false);
        }

        async void Item_Clicked(object sender, System.EventArgs e)
        {
            Member item = ((sender as Button).CommandParameter as Member);

            if (item == null)
                throw new NullReferenceException();

            await ItemDetailOpener.Open(Navigation, item).ConfigureAwait(false);
        }

        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            Item item = (sender as ViewCell).BindingContext as Item;

            if (item != null)
            {
                await ItemDetailOpener.Open(Navigation, item).ConfigureAwait(false);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (AllItems == null)
            {
                AllItems = LocalDataStore.GetItems<Member>(this.groupId, this.album).Result;

                Categories.Clear();

                foreach (Member item in AllItems)
                {
                    foreach (String itemCategory in item.Categories)
                    {
                        if (!String.IsNullOrEmpty(itemCategory) && !Categories.Contains(itemCategory))
                        {
                            Categories.Add(itemCategory);
                        }
                    }
                }

                Categories.Sort();

                Categories.Insert(0, AllCategories);

                CategoryPicker.ItemsSource = Categories;
                CategoryPicker.SelectedIndex = 0;
            }
        }

        async void Set_Favorite_Clicked(object sender, System.EventArgs e)
        {
            Item item = ((sender as Button).CommandParameter as Item);
            try
            {
                await LocalDataStore.SetFavorite(item).ConfigureAwait(false);
            }
            catch (Exception)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Ошибка", "Произошла ошибка при попытке добавить в избранное", "OK");
                });
            }
        }

        async void Unset_Favorite_Clicked(object sender, System.EventArgs e)
        {
            Item item = ((sender as Button).CommandParameter as Item);
            try
            {
                await LocalDataStore.UnsetFavorite(item).ConfigureAwait(false);
            }
            catch (Exception)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Ошибка", "Произошла ошибка при попытке исключить из избранного", "OK");
                });
            }
        }
    }
}

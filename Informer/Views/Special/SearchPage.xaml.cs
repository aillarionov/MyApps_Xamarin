using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services;
using Informer.Utils;
using Informer.ViewModels;
using Xamarin.Forms;

namespace Informer.Views.Special
{
    public partial class SearchPage : ContentPage
    {

        public ObservableCollection<IItem> Items { get; set; }
        private int groupId;
        private CancellationTokenSource throttleCts = new CancellationTokenSource();

        public SearchPage(int groupId)
        {
            this.groupId = groupId;
            InitializeComponent();
            Title = "Поиск";
            Items = new ObservableCollection<IItem>();

            BindingContext = this;

            LoadGroupConfig();
        }

        private void LoadGroupConfig()
        {
            ExpoImage.Source = new UriImageSource
            {
                Uri = App.group.Logo.Uri,
                CacheValidity = App.imageCacheTime
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void Search() 
        {
            String text = "%" + SearchQuery.Text.ToLower() + "%";
            List<IItem> items = await LocalDataStore.SearchItems(this.groupId, text).ConfigureAwait(false);

            Items.Clear();

            foreach (IItem item in items)
            {
                Items.Add(item);
            }
        }

        void Handle_Search(object sender, System.EventArgs e) 
        {
            Interlocked.Exchange(ref this.throttleCts, new CancellationTokenSource()).Cancel();
            Task.Delay(App.queryDelay, this.throttleCts.Token) // throttle time
                .ContinueWith(
                    delegate { this.Search(); },
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

        async void Item_Clicked(object sender, System.EventArgs e)
        {
            Item item = ((sender as Button).CommandParameter as Item);

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

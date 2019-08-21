using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services;
using Informer.Utils;
using Informer.ViewModels;
using Xamarin.Forms;

namespace Informer.Views.Special
{
    public partial class FavoritesPage : ContentPage
    {
        private int groupId;

        ItemsViewModel<Item> viewModel;

        public FavoritesPage(int groupId)
        {
            this.groupId = groupId;
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel<Item>(groupId, new Album());
            viewModel.Title = "Избранное";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var items = LocalDataStore.GetFavoriteItems(groupId).Result;
            viewModel.Items.Clear();

            foreach (Item item in items)
            {
                viewModel.Items.Add(item);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.Items.Clear();
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

        async void Unset_Favorite_Clicked(object sender, System.EventArgs e)
        {
            Item item = ((sender as Button).CommandParameter as Item);
            try
            {
                await LocalDataStore.UnsetFavorite(item).ConfigureAwait(false);
                viewModel.Items.Remove(item);
            }
            catch (Exception)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Ошибка", "Произошла ошибка при попытке исключить из избранного", "OK");
                });
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
    }
}

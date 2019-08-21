using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services;
using Informer.Utils;
using Informer.ViewModels;
using Xamarin.Forms;

namespace Informer.Views
{
    public partial class ItemsPage : ContentPage
    {
        

        ItemsViewModel<Item> viewModel;

        public ItemsPage(int groupId, Album album)
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel<Item>(groupId, album);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
            {
                Task.Run(() => { 
                    viewModel.LoadItemsCommand.Execute(null); 
                }) ;
            }
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
            catch(Exception)
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
            catch(Exception)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Ошибка", "Произошла ошибка при попытке исключить из избранного", "OK");
                });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services;
using Informer.Utils;
using Informer.ViewModels;
using Xamarin.Forms;

namespace Informer.Views
{
    public partial class NewsPage : ContentPage
    {
        public ObservableCollection<News> Items { get; set; }
        int groupId;


        public NewsPage(int groupId)
        {
            this.groupId = groupId;
            InitializeComponent();

            Title = "Новости";
            Items = new ObservableCollection<News>();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this.Items.Count == 0)
            {
                Task.Run(() => { 
                    List<News> items = LocalDataStore.GetItems<News>(this.groupId, null).Result;
                    this.Items.Clear();
                    foreach (News item in items) 
                    {
                        this.Items.Add(item);    
                    }
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

using System;
using System.Threading;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services;
using Informer.Services.RPC;
using Informer.Utils;
using Informer.ViewModels;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Informer.Views
{
    public partial class InitialPage : ContentPage
    {
        int groupId;
        bool StartClicked = false;

        public InitialPage(int groupId)
        {
            this.groupId = groupId;
            this.Title = "Начало работы";

            InitializeComponent();

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


        async void Handle_Start_Clicked(object sender, System.EventArgs e)
        {
            if (StartClicked) 
            {
                return;    
            }
            StartClicked = true;

            try
            {
                App.config = new Config
                {
                    GroupId = groupId,
                    IsPresenter = IsPresenter.IsToggled,
                    IsVisitor = !IsPresenter.IsToggled,
                    ReceivePush = true
                };


                await DataLoader.UpdateGroup(App.config, new ObservableProgress(), default(CancellationToken)).ConfigureAwait(false);

                if (Preload.IsToggled) 
                {
                    App.SetPage(new LoadingPage(this.groupId, new Action(() => { Device.BeginInvokeOnMainThread(() => App.ContinueWithGroup()); })));   
                } else {
                    Device.BeginInvokeOnMainThread(() => App.ContinueWithGroup());
                }

            }
            catch (Exception)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Ошибка", "Произошла ошибка при попытке загрузить данные с сервера. Попробуйте запустить программу позже", "OK");
                });
            }

            StartClicked = false;
        }
    }
}

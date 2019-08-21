using System;
using System.Threading;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services;
using Informer.Services.RPC;
using Informer.Utils;
using Informer.ViewModels;
using Xamarin.Forms;

namespace Informer.Views
{
    public partial class LoadingPage : ContentPage
    {
        int groupId;
        Action action;
        ObservableProgress Progress;
        CancellationTokenSource source = new CancellationTokenSource();

        public LoadingPage(int groupId, Action action)
        {
            this.groupId = groupId;
            this.Title = "Загрузка данных";
            this.action = action;

            InitializeComponent();

            BindingContext = this;

            Progress = new ObservableProgress();

            Progress.PropertyChanged += (sender, e) => { 
                Device.BeginInvokeOnMainThread(() =>
                {
                    BarProgress.Progress = Progress.FloatProgress; 
                });
            };

            LoadGroupConfig();

            Task.Run(() => LoadData());
        }

        private void LoadGroupConfig()
        {
            ExpoImage.Source = new UriImageSource
            {
                Uri = App.group.Logo.Uri,
                CacheValidity = App.imageCacheTime
            };
        }

        public async void LoadData() 
        {
            try
            {
                await DataLoader.CacheImages(App.config, Progress, source.Token).ConfigureAwait(false);
            }
            /*catch (OperationCanceledException) 
            {
                
            }*/
            catch (Exception)
            {

            }
            if (this.action != null)
            {
                this.action.Invoke();
            }

           
        }

        void Handle_Break_Clicked(object sender, System.EventArgs e)
        {
            source.Cancel(true);
        }


        protected override void OnDisappearing()
        {
            this.action = null;
            //source.Cancel(true);
        }

}
}

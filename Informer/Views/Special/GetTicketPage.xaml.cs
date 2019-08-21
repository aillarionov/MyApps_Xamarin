using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Informer.Services.RPC;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Informer.Views.Special
{
    public partial class GetTicketPage : ContentPage
    {
        internal const string CacheName = "TicketCache";
        static readonly IIsolatedStorageFile Store = Device.PlatformServices.GetUserStoreForApplication();

        private int groupId;

        public GetTicketPage(int groupId)
        {
            this.groupId = groupId;

            InitializeComponent();

            this.Title = "Билет";

            BindingContext = this;

            if (!Store.GetDirectoryExistsAsync(CacheName).Result)
                Store.CreateDirectoryAsync(CacheName).Wait();


            if (Store.GetFileExistsAsync(Path.Combine(CacheName, this.groupId.ToString())).Result)
            {
                ShowExistsTicket();
            }
            else 
            {
                ShowNoTicket();
            }
        }

        public void RetrieveTicket() 
        {
            try
            {
                byte[] data = RemoteFunctions.GetTicket(this.groupId, default(CancellationToken)).Result;

                if (data == null)
                {
                    return;
                }

                Stream stream = new MemoryStream(data);
                Stream writeStream = Store.OpenFileAsync(Path.Combine(CacheName, this.groupId.ToString()), Xamarin.Forms.Internals.FileMode.Create, Xamarin.Forms.Internals.FileAccess.Write).Result;


                stream.CopyToAsync(writeStream, 16384, default(CancellationToken)).Wait();

                if (writeStream != null)
                    writeStream.Dispose();

                stream.Dispose();

                ShowExistsTicket();
            }
            catch (Exception)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Ошибка", "Произошла ошибка при попытке получить билет. Попробуйте позже", "OK");
                });
            }


        }

        public void ShowExistsTicket()
        {
            Image img = new Image 
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromStream(() =>
                {
                    Stream fileStream = Store.OpenFileAsync(Path.Combine(CacheName, this.groupId.ToString()), Xamarin.Forms.Internals.FileMode.Open, Xamarin.Forms.Internals.FileAccess.Read).Result;
                    return fileStream;
                })
            };


            StackLayout stack = new StackLayout
            {
                Spacing = 0,
                Orientation = StackOrientation.Vertical
            };

            stack.Children.Add(img);

            Content = stack;
        }

        public void ShowNoTicket()
        {
            Button button = new Button
            {
                Text = "Получить билет",
                Margin = new Thickness(5)
            };
            button.Clicked += (sender, e) => { RetrieveTicket(); };


            StackLayout stack = new StackLayout
            {
                Spacing = 0,
                Orientation = StackOrientation.Vertical,
                StyleClass = new List<String> { "Foreground" }
            };

            stack.Children.Add(new Label{ Margin= new Thickness(5), Text="Получите бесплатный билет, нажав на кнопку и покажите его на входе. Вход по билету в приложении бесплатный во все часы работы выставки." });

            stack.Children.Add(button);

            Content = stack;
        }

    }
}

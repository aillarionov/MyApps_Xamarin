using System;
using Informer.Models;
using Informer.Services;
using Informer.ViewModels;
using Xamarin.Forms;

namespace Informer.Views
{
    public partial class AboutPage : ContentPage
    {
        ItemDetailViewModel<FixedPost> viewModel;
        int groupId;

        public AboutPage(int groupId)
        {
            this.groupId = groupId;

            InitializeComponent();

            viewModel = new ItemDetailViewModel<FixedPost>(LocalDataStore.GetFixedPost(groupId).Result);
            viewModel.Title = "Главная";
            BindingContext = this.viewModel;

            GenerateOtherPhotos(); 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        void GenerateOtherPhotos()
        {
            if (viewModel.Item != null)
            {
                foreach (Photo photo in viewModel.Item.Photos)
                {
                    Xamarin.Forms.Image image = new Xamarin.Forms.Image
                    {
                        Source = new UriImageSource
                        {
                            Uri = photo.Uri,
                            CacheValidity = new TimeSpan(365, 0, 0, 0)
                        },
                        BackgroundColor = Color.Red,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Margin = new Thickness(0)
                    };

                    OtherPhotos.Children.Add(image);
                }
            }
        }
    }
}

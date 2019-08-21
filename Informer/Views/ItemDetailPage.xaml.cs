using System;
using System.Threading.Tasks;
using Informer.Models;
using Informer.ViewModels;
using Xamarin.Forms;

              
namespace Informer.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel<Item> viewModel;

        public ItemDetailPage(ItemDetailViewModel<Item> viewModel)
        {
            InitializeComponent();

            viewModel.Title = "";

            BindingContext = this.viewModel = viewModel;

            GenerateOtherPhotos();
        }
     
        void GenerateOtherPhotos()
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
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Margin = new Thickness(0)
                };

                OtherPhotos.Children.Add(image);
            }

            if (Device.RuntimePlatform == Device.Android)
            {

               
            }
        }

    }
}

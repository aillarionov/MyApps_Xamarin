using System;
using System.Threading.Tasks;
using Informer.Models;
using Informer.ViewModels;
using Informer.Utils;
using Xamarin.Forms;

              
namespace Informer.Views.Special
{
    public partial class MemberDetailPage : ContentPage
    {
        ItemDetailViewModel<Member> viewModel;

        public MemberDetailPage(ItemDetailViewModel<Member> viewModel)
        {
            InitializeComponent();
                
            viewModel.Title = (viewModel.Item as Member).Name;

            BindingContext = this.viewModel = viewModel;

            GenerateOtherPhotos();

            GenerateContacts();
        }

        void GenerateContacts()
        {
            foreach (View v in ContactsGenerator.Generate(viewModel.Item)) 
            {
                Contacts.Children.Add(v);    
            }
        }

        void GenerateOtherPhotos()
        {
            /*
            foreach (Photo photo in viewModel.Item.Photos)
            {
                Image image = new Image
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

               
            }*/
        }

    }
}

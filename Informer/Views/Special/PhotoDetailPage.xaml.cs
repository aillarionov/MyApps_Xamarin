using System;
using System.Threading.Tasks;
using Informer.Models;
using Informer.ViewModels;
using Xamarin.Forms;

              
namespace Informer.Views.Special
{
    public partial class PhotoDetailPage : ContentPage
    {
        ItemDetailViewModel<Models.Image> viewModel;

        public PhotoDetailPage(ItemDetailViewModel<Models.Image> viewModel)
        {
            InitializeComponent();

            viewModel.Title = "";

            BindingContext = this.viewModel = viewModel;
        }
     
       

    }
}

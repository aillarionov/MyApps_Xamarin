using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services;
using Informer.Utils;
using Informer.ViewModels;
using Xamarin.Forms;

namespace Informer.Views.Special
{
    public partial class PhotoPage : ContentPage
    {
        int groupId;
        ItemsViewModel<Models.Image> viewModel;

        public PhotoPage(int groupId, Album album)
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel<Models.Image>(groupId, album);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
            {
                Task.Run(() => {
                    viewModel.LoadItemsCommand.Execute(null);
                });
            }
        }

        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            ViewCell cell = sender as ViewCell;
            Models.Image image = cell.BindingContext as Models.Image;
            await ItemDetailOpener.Open(Navigation, image).ConfigureAwait(false);
        }
    }
}

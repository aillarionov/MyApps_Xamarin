using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class GroupsPage : ContentPage
    {
        public ObservableCollection<SimpleGroup> Items { get; set; }

        public GroupsPage()
        {
            InitializeComponent();

            Title = "Выбор группы";
            Items = new ObservableCollection<SimpleGroup>();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this.Items.Count == 0)
            {
                Task.Run(async () => {
                    List<SimpleGroup> items = await RemoteFunctions.GetGroups(default(CancellationToken)).ConfigureAwait(false);
                    this.Items.Clear();
                    foreach (SimpleGroup item in items) 
                    {
                        this.Items.Add(item);    
                    }
                });
            }
        }


        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            SimpleGroup group = ((sender as ViewCell).BindingContext as SimpleGroup);

            if (group != null) 
            {
                try 
                {
                    await LocalDataStore.UpdateGroup(group.Id, default(CancellationToken)).ConfigureAwait(false);
                    Group g = await LocalDataStore.GetGroup(group.Id).ConfigureAwait(false);
                    App.group = g;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        App.ContinueWithGroup();
                    });
                }
                catch 
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Ошибка", "Произошла ошибка при попытке загрузить данные с сервера. Попробуйте запустить программу позже", "OK");
                    });
                }


            }
        }
    }
}

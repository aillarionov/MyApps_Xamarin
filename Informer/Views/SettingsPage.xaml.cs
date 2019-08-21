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
    public partial class SettingsPage : ContentPage
    {
        public class SettingItem
        {
            public Group Group { get; set; }
            public Config Config { get; set; }
        }


        public ObservableCollection<SettingItem> Items { get; set; }


        public SettingsPage()
        {
            InitializeComponent();

            Title = "Настройки";
            Items = new ObservableCollection<SettingItem>();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this.Items.Count == 0)
            {
                Task.Run(async () => {
                    List<Group> groups = await LocalDataStore.GetGroups().ConfigureAwait(false);
                    List<Config> configs = await LocalDataStore.GetConfigs().ConfigureAwait(false);

                    this.Items.Clear();

                    foreach (Config config in configs) 
                    {
                        SettingItem item = new SettingItem();

                        item.Config = config;

                        foreach (Group group in groups) 
                        {
                            if (group.Id == config.GroupId) 
                            {
                                item.Group = group;
                                break;
                            }
                        }
                        if (item.Group != null) 
                        {
                            this.Items.Add(item);    
                        }
                    }
                });
            }
        }

        async void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            SettingItem item = (sender as Switch).BindingContext as SettingItem;

            if (item.Config.IsVisitor == e.Value)
            {
                item.Config.IsVisitor = !e.Value;

                await LocalDataStore.SaveConfig(item.Config).ConfigureAwait(true);

                App.token = null;
                App.SendUpdatedToken();
            }
        }

        async void Handle_Load_Clicked(object sender, System.EventArgs e)
        {
            SettingItem item = ((sender as Button).CommandParameter as SettingItem);

            await Navigation.PushAsync(new LoadingPage(item.Group.Id, new Action(() =>
            {
                Device.BeginInvokeOnMainThread(() => { Navigation.PopAsync();} );
            }))).ConfigureAwait(false);
        }

        void Handle_Change_Clicked(object sender, System.EventArgs e)
        {
            SettingItem item = ((sender as Button).CommandParameter as SettingItem);
            App.group = item.Group;

            Device.BeginInvokeOnMainThread(() =>
            {
                App.ContinueWithGroup();
            });

        }

        void Handle_Delete_Clicked(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if ("Удалить" == await DisplayActionSheet("Будут удалены относящиеся к этой выставке настроки, избранное и данные для работы оффлайн", "Не удалять", "Удалить", "").ConfigureAwait(false))
                {
                    SettingItem item = ((sender as Button).CommandParameter as SettingItem);

                    try
                    {
                        await DataLoader.DeleteImages(item.Config, new ObservableProgress(), default(CancellationToken)).ConfigureAwait(false);
                        await DataLoader.DeleteGroup(item.Config, new ObservableProgress(), default(CancellationToken)).ConfigureAwait(false);
                        if (item.Group.Id == App.config.GroupId)
                        {
                            App.token = null;
                            App.SendUpdatedToken();
                            App.SetPage(new GroupsPage());
                        }
                        else
                        {
                            this.Items.Remove(item);
                            App.token = null;
                            App.SendUpdatedToken();
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services;
using Xamarin.Forms;

namespace Informer.ViewModels
{
    public class ItemsViewModel<T> : BaseViewModel where T : Item
    {
        private int groupId;

        public Album Album { get; set; }

        public ObservableCollection<T> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel(int groupId, Album album)
        {
            this.Album = album;
            this.groupId = groupId;
            Title = Album.Title;
            Items = new ObservableCollection<T>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
        }

        public void ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var items = LocalDataStore.GetItems<T>(this.groupId, this.Album).Result;

                Items.Clear();

                foreach (T item in items)
                {
                    Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

       
    }
}

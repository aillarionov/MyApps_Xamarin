using System;
using System.Threading.Tasks;
using Informer.Models;
using Informer.ViewModels;
using Informer.Views;
using Informer.Views.Special;
using Xamarin.Forms;

namespace Informer.Utils
{
    public static class ItemDetailOpener
    {

        public static async Task Open(INavigation Navigation, IItem item) 
        {
            switch (item.GetType().Name)
            {
                case ("News"):
                case ("Item"):
                    await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel<Item>(item as Item)), true).ConfigureAwait(false);
                    break;

                case ("Member"):
                    await Navigation.PushAsync(new MemberDetailPage(new ItemDetailViewModel<Member>(item as Member)), true).ConfigureAwait(false);
                    break;

                case ("Image"):
                    await Navigation.PushAsync(new PhotoDetailPage(new ItemDetailViewModel<Models.Image>(item as Models.Image)), true).ConfigureAwait(false);
                    break;

                default:
                    throw new Exception("Тип [" + item.GetType().Name + "] не найден");
            }

        }
    }
}

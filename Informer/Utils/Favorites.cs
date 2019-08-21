using System;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services;
using Informer.Services.DataStore;

namespace Informer.Utils
{
    public static class Favorites
    {
        /*
        public async static Task<bool> AddToFavorite(int groupId, Album album, Item item) 
        {
            Favorite favorite = new Favorite
            {
                GroupId = groupId,
                AlbumId = album.Id,
                ItemId = item.Id
            };

            await FavoritesDataStore.PutFavorite(favorite).ConfigureAwait(false);

            if (favorite.Id > 0)
            {
                item.Favorite = favorite;
                return true;
            }
            else
            {
                return false;

            }

        }

        public async static Task<bool> RemoveFromFavorite(Item item)
        {
            // Удаление
            await FavoritesDataStore.RemoveFavorite(item.Favorite).ConfigureAwait(false);

            if (item.Favorite.Id == 0) 
            {
                item.Favorite = null;
                return true;
            }
            else
            {
                return false;

            }
        }
        */

    }
}

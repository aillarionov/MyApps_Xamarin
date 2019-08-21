using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informer.Models;
using SQLite;

namespace Informer.Services.DataStore
{
    public class FavoritesDataStore
    {
        private readonly SQLiteAsyncConnection database = LocalDataStore.Database;

        public FavoritesDataStore()
        {
            Create().Wait();
        }

        public async Task Create() 
        {
            await database.CreateTableAsync<Favorite>().ConfigureAwait(false);
        }

        public async Task<List<Favorite>> GetFavorites()
        {
            return await database.Table<Favorite>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<Favorite>> GetFavorites(int GroupId)
        {
            return await database.Table<Favorite>().Where(i => i.GroupId == GroupId).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<Favorite>> GetFavorites(int GroupId, int AlbumId)
        {
            return await database.Table<Favorite>().Where(i => i.GroupId == GroupId && i.AlbumId == AlbumId).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<Favorite>> GetFavorites(int GroupId, int AlbumId, int ItemId)
        {
            return await database.Table<Favorite>().Where(i => i.GroupId == GroupId && i.AlbumId == AlbumId && i.ItemId == ItemId).ToListAsync().ConfigureAwait(false);
        }

        public async Task PutFavorite(Favorite item) 
        {
            await database.InsertAsync(item).ConfigureAwait(false);
        }

        public async Task RemoveFavorite(Favorite item)
        {
            await database.DeleteAsync(item).ConfigureAwait(false);
        }

        public async Task<int> Clear(int groupId)
        {
            return await database.ExecuteAsync("DELETE FROM [Favorite] WHERE [groupId] = ?;", groupId).ConfigureAwait(false);
        }

    }
}

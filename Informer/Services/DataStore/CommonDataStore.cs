using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informer.Models;
using SQLite;

namespace Informer.Services.DataStore
{
    public class CommonDataStore<T> : IDataStore<T> where T : Item, new()
    {
        private readonly SQLiteAsyncConnection database = LocalDataStore.Database;

        public CommonDataStore()
        {
            Create().Wait();
        }

        public async Task Create()
        {
            await database.CreateTableAsync<T>().ConfigureAwait(false);
        }

        public async Task<List<T>> GetItems(int groupId)
        {
            return await database.Table<T>().Where(i => i.GroupId == groupId).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<T>> GetItems(int groupId, Album album)
        {
            return await database.Table<T>().Where(i => i.GroupId == groupId && i.AlbumId == album.Id).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<T>> SearchItems(int groupId, String text)
        {
            return await database.QueryAsync<T>("SELECT * FROM [" + typeof(T).Name + "] WHERE [GroupId] = ? AND [Raw] LIKE ? LIMIT 20", groupId, text.ToLower()).ConfigureAwait(false);
        }

        public async Task<List<T>> GetFavoriteItems(int groupId)
        {
            return await database.QueryAsync<T>(
                "SELECT i.* " +
                "FROM [" + typeof(T).Name + "] i " +
                "INNER JOIN [Favorite] f " +
                "           ON f.[GroupId] = i.[GroupId] " +
                "              AND f.[AlbumId] = i.[AlbumId] " +
                "              AND f.[ItemId] = i.[Id]" +
                "WHERE i.[groupId] = ?;", groupId
            ).ConfigureAwait(false);
        }

        public async Task<int> Clear(int groupId) 
        {
            return await database.ExecuteAsync("DELETE FROM [" + typeof(T).Name + "] WHERE [GroupId] = ?;", groupId).ConfigureAwait(false);
        }

        public async Task<int> Clear(int groupId, Album album)
        {
            
            return await database.ExecuteAsync("DELETE FROM [" + typeof(T).Name + "] WHERE [GroupId] = ? AND [AlbumId] = ?;", groupId, album.Id).ConfigureAwait(false);
        }

        public async Task<int> PutItem(Item item)
        {
            return await database.InsertAsync(item).ConfigureAwait(false);
        }

        public async Task<int> PutItems(List<T> items)
        {
            return await database.InsertAllAsync(items).ConfigureAwait(false);
        }

        public async Task<T> GetItem(int groupId, Album album, int id)
        {
            return await database.Table<T>().Where(i => i.GroupId == groupId && i.AlbumId == album.Id && i.Id == id).FirstAsync().ConfigureAwait(false);
        }

    }
}

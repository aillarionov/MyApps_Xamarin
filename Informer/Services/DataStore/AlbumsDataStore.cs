using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informer.Models;
using SQLite;

namespace Informer.Services.DataStore
{
    public class AlbumsDataStore
    {
        private readonly SQLiteAsyncConnection database = LocalDataStore.Database;

        public AlbumsDataStore()
        {
            Create().Wait();
        }

        public async Task Create()
        {
            await database.CreateTableAsync<Album>().ConfigureAwait(false);
        }

        public async Task<List<Album>> GetAlbums()
        {
            return await database.Table<Album>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<Album>> GetAlbums(int GroupId)
        {
            return await database.Table<Album>().Where(i => i.GroupId == GroupId).ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> Clear(int groupId)
        {
            return await database.ExecuteAsync("DELETE FROM [Album] WHERE [GroupId] = ?;", groupId).ConfigureAwait(false);
        }

        public async Task<int> PutAlbum(Album item) 
        {
            int id = await database.InsertAsync(item).ConfigureAwait(false);
            item.Id = id;
            return id;
        }

        public async Task<int> PutAlbums(List<Album> items)
        {
            return await database.InsertAllAsync(items).ConfigureAwait(false);
        }

        public async Task<int> RemoveAlbum(Album item)
        {
            int id = await database.DeleteAsync(item).ConfigureAwait(false);
            if (id > 0) 
            {
                item.Id = 0;    
            }
            return id;
        }

    }
}

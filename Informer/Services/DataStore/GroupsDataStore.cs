using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informer.Models;
using SQLite;

namespace Informer.Services.DataStore
{
    public class GroupsDataStore
    {
        private readonly SQLiteAsyncConnection database = LocalDataStore.Database;

        public GroupsDataStore()
        {
            Create().Wait();
        }

        public async Task Create()
        {
            await database.CreateTableAsync<Group>().ConfigureAwait(false);
        }

        public async Task<List<Group>> GetGroups()
        {
            return await database.Table<Group>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<Group> GetGroup(int GroupId)
        {
            return await database.Table<Group>().Where(i => i.Id == GroupId).FirstAsync().ConfigureAwait(false);
        }

        public async Task PutGroup(Group item)
        {
            await database.InsertOrReplaceAsync(item).ConfigureAwait(false);
        }

        public async Task RemoveGroup(Group item)
        {
            await database.DeleteAsync(item).ConfigureAwait(false);
        }

        public async Task<int> Clear(int groupId)
        {
            return await database.ExecuteAsync("DELETE FROM [Group] WHERE [id] = ?;", groupId).ConfigureAwait(false);
        }

    }
}

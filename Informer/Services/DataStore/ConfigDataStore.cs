using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informer.Models;
using SQLite;

namespace Informer.Services.DataStore
{
    public class ConfigDataStore
    {
        private readonly SQLiteAsyncConnection database = LocalDataStore.Database;

        public ConfigDataStore()
        {
            Create().Wait();
        }

        public async Task Create()
        {
            await database.CreateTableAsync<Config>().ConfigureAwait(false);
        }

        public async Task<List<Config>> GetConfigs()
        {
            return await database.Table<Config>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<Config> GetConfig(int GroupId)
        {
            return await database.Table<Config>().Where(i => i.GroupId == GroupId).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task SaveConfig(Config config) 
        {
            await database.InsertOrReplaceAsync(config).ConfigureAwait(false);
        }

        public async Task RemoveConfig(Config config)
        {
            await database.DeleteAsync(config).ConfigureAwait(false);
        }

    }
}

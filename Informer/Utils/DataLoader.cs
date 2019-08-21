using System;
using System.Threading;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services;
using Informer.Services.RPC;

namespace Informer.Utils
{
    public static class DataLoader
    {
        public static async Task LoadGroup(Config config, ObservableProgress progress, CancellationToken cancellationToken) 
        {
            String lastChange = await RemoteFunctions.GetLastChange(config.GroupId, cancellationToken).ConfigureAwait(false);

            await LocalDataStore.LoadGroup(config.GroupId, cancellationToken).ConfigureAwait(false);

            config.LastChange = lastChange;

            await LocalDataStore.SaveConfig(App.config).ConfigureAwait(false);
        }

        public static async Task UpdateGroup(Config config, ObservableProgress progress, CancellationToken cancellationToken) 
        {
            String lastChange = await RemoteFunctions.GetLastChange(config.GroupId, cancellationToken).ConfigureAwait(false);

            if (config.LastChange != lastChange) 
            {
                await LoadGroup(config, progress, cancellationToken).ConfigureAwait(false);
            }
        }

        public static async Task CacheImages(Config config, ObservableProgress progress, CancellationToken cancellationToken)
        {
            await LocalDataStore.CacheImages(config.GroupId, progress, cancellationToken).ConfigureAwait(false);
        }

        public static async Task DeleteImages(Config config, ObservableProgress progress, CancellationToken cancellationToken)
        {
            await LocalDataStore.DeleteImages(config.GroupId, progress, cancellationToken).ConfigureAwait(false);
        }

        public static async Task DeleteGroup(Config config, ObservableProgress progress, CancellationToken cancellationToken)
        {
            await LocalDataStore.ClearGroup(config).ConfigureAwait(false);
        }
    }
}

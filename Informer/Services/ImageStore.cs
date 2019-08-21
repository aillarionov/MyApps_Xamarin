using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Threading.Tasks;
using Informer.Services.RPC;
using Informer.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Informer.Services
{
    public static class ImageStore
    {
        /* >>>>>> From UriImageSource */
        private static TimeSpan CacheValidity = App.imageCacheTime;
        private const string CacheName = "ImageLoaderCache";
        private static readonly IIsolatedStorageFile Store = Device.PlatformServices.GetUserStoreForApplication();
        /* <<<<<< From UriImageSource */

        static ImageStore() 
        {
            if (!Store.GetDirectoryExistsAsync(CacheName).Result)
                Store.CreateDirectoryAsync(CacheName).Wait();
        }


        public static async Task ClearImageCache(List<Uri> uris, ObservableProgress progress, CancellationToken cancellationToken)
        {
            progress.Total = uris.Count;
            progress.Current = 0;

            foreach (Uri uri in uris)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await DeleteImage(uri, cancellationToken).ConfigureAwait(false);
                progress.Current++;
            }
        }

        public static async Task StroreImageCache(List<Uri> uris, ObservableProgress progress, CancellationToken cancellationToken)
        {
            progress.Total = uris.Count;
            progress.Current = 0;

            foreach (Uri uri in uris) 
            {
                cancellationToken.ThrowIfCancellationRequested();
                await CacheImage(uri, cancellationToken).ConfigureAwait(false);
                progress.Current++;
            }
        }

        private static async Task CacheImage(Uri uri, CancellationToken cancellationToken)
        {
            String key = GetCacheKey(uri);

            if (!await GetHasLocallyCachedCopyAsync(key).ConfigureAwait(false))
            {
                await DownloadFile(key, uri, cancellationToken).ConfigureAwait(false);
            }

            #if DEBUG
            Console.WriteLine("Cached {0}", uri.AbsoluteUri);
            #endif
        }

        private static async Task DeleteImage(Uri uri, CancellationToken cancellationToken)
        {
            String key = GetCacheKey(uri);

            String fileName = Path.Combine(CacheName, key);

            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

            if (await Store.GetFileExistsAsync(fileName).ConfigureAwait(false))
            {
                store.DeleteFile(fileName);

                #if DEBUG
                Console.WriteLine("Deleted {0}", uri.AbsoluteUri);
                #endif
            }

           
        }

        private static string GetCacheKey(Uri uri)
        {
            return Device.PlatformServices.GetMD5Hash(uri.AbsoluteUri);
        }

        private static async Task<bool> GetHasLocallyCachedCopyAsync(string key, bool checkValidity = true)
        {
            DateTime now = DateTime.UtcNow;
            DateTime? lastWriteTime = await GetLastWriteTimeUtcAsync(key).ConfigureAwait(false);
            return lastWriteTime.HasValue && now - lastWriteTime.Value < CacheValidity;
        }

        private static async Task<DateTime?> GetLastWriteTimeUtcAsync(string key)
        {
            string path = Path.Combine(CacheName, key);
            if (! await Store.GetFileExistsAsync(path).ConfigureAwait(false))
                return null;

            return (await Store.GetLastWriteTimeAsync(path).ConfigureAwait(false)).UtcDateTime;
        }

        private static async Task DownloadFile(String key, Uri uri, CancellationToken cancellationToken) 
        {
            try
            {
                Stream stream = await RemoteFunctions.LoadPhoto(uri, cancellationToken).ConfigureAwait(false);
                Stream writeStream = await Store.OpenFileAsync(Path.Combine(CacheName, key), Xamarin.Forms.Internals.FileMode.Create, Xamarin.Forms.Internals.FileAccess.Write).ConfigureAwait(false);

                cancellationToken.ThrowIfCancellationRequested();

                await stream.CopyToAsync(writeStream, 16384, cancellationToken);

                if (writeStream != null)
                    writeStream.Dispose();

                stream.Dispose();
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) 
                {
                    throw e;    
                }

                #if DEBUG
                Console.WriteLine("Exception on image cache {0} {1}", uri.AbsoluteUri, e.Message);
                #endif
            }
        }
    }
}

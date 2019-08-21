using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services.DataStore;
using Informer.Services.RPC;
using Informer.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using SQLite;
using System.Text;

namespace Informer.Services
{
    public static class LocalDataStore
    {
        private static SQLiteAsyncConnection database;
        private static readonly IIsolatedStorageFile Store = Device.PlatformServices.GetUserStoreForApplication();

        private static readonly String localDB = "localdb.sqlite";
        private static readonly String CurrentGroupConfigFile = "current_group.txt";

        private static int newsAlbumId = -10;

        private static CommonDataStore<FixedPost> FixedPost { get; set; } = new CommonDataStore<FixedPost>();
        private static CommonDataStore<Item> Items { get; set; } = new CommonDataStore<Item>();
        private static CommonDataStore<Member> Members { get; set; } = new CommonDataStore<Member>();
        private static CommonDataStore<News> News { get; set; } = new CommonDataStore<News>();
        private static CommonDataStore<Models.Image> Images { get; set; } = new CommonDataStore<Models.Image>();
        private static AlbumsDataStore Albums { get; set; } = new AlbumsDataStore();
        private static FavoritesDataStore Favorites { get; set; } = new FavoritesDataStore();
        private static ConfigDataStore Config { get; set; } = new ConfigDataStore();
        private static GroupsDataStore Groups { get; set; } = new GroupsDataStore();

        static LocalDataStore() 
        {
        }

        public static SQLiteAsyncConnection Database
        {
            get
            {
                if (database == null)
                {
                    database = new SQLiteAsyncConnection(GetDBPath());
                }

                return database;
            }
        }

        public static async Task Create()
        {
            await Config.Create().ConfigureAwait(false);
            await Groups.Create().ConfigureAwait(false);
            await FixedPost.Create().ConfigureAwait(false);
            await Favorites.Create().ConfigureAwait(false);
            await News.Create().ConfigureAwait(false);
            await Images.Create().ConfigureAwait(false);
            await Members.Create().ConfigureAwait(false);
            await Items.Create().ConfigureAwait(false);
            await Albums.Create().ConfigureAwait(false);
        }

        private static async Task Clear(int groupId) 
        {
            await FixedPost.Clear(groupId).ConfigureAwait(false);
            await Members.Clear(groupId).ConfigureAwait(false);
            await Images.Clear(groupId).ConfigureAwait(false);
            await Items.Clear(groupId).ConfigureAwait(false);
            await Albums.Clear(groupId).ConfigureAwait(false);
        }


        public static async Task ClearGroup(Config config)
        {
            try
            {
                Group group = await GetGroup(config.GroupId).ConfigureAwait(false);

                List<Uri> uris = new List<Uri>();

                foreach (KeyValuePair<String, String> kvp in group.Plan)
                {
                    uris.Add(new Uri(kvp.Value));
                }

                await ImageStore.StroreImageCache(uris, new ObservableProgress(), default(CancellationToken)).ConfigureAwait(false);
            }
            catch (Exception)
            {

            }


            await FixedPost.Clear(config.GroupId).ConfigureAwait(false);
            await Members.Clear(config.GroupId).ConfigureAwait(false);
            await Images.Clear(config.GroupId).ConfigureAwait(false);
            await Items.Clear(config.GroupId).ConfigureAwait(false);
            await Albums.Clear(config.GroupId).ConfigureAwait(false);
            await News.Clear(config.GroupId).ConfigureAwait(false);
            await Favorites.Clear(config.GroupId).ConfigureAwait(false);
            await Groups.Clear(config.GroupId).ConfigureAwait(false);
            await Config.RemoveConfig(config).ConfigureAwait(false);
        }

        public static async Task<List<IItem>> GetFavoriteItems(int groupId) 
        {
            List<IItem> items = new List<IItem>();

            foreach (IItem item in await Items.GetFavoriteItems(groupId).ConfigureAwait(false)) 
            {
                items.Add(item);
            }

            foreach (IItem item in await Members.GetFavoriteItems(groupId).ConfigureAwait(false))
            {
                items.Add(item);
            }

            foreach (IItem item in await News.GetFavoriteItems(groupId).ConfigureAwait(false))
            {
                items.Add(item);
            }

            await ApplyFavorites(groupId, items).ConfigureAwait(false);

            return items;
        }

        public static async Task SetFavorite(Item item)
        {
            Favorite f = Favorite.FromItem(item);
            await Favorites.PutFavorite(f).ConfigureAwait(false);
            item.Favorite = f;
        }

        public static async Task UnsetFavorite(Item item)
        {
            await Favorites.RemoveFavorite(item.Favorite).ConfigureAwait(false);
            item.Favorite = null;
        }

        public static async Task<List<T>> GetItems<T>(int groupId, Album album) where T : Item
        {
            List<T> items;

            switch (typeof(T).Name) 
            {
                case ("Item"):
                    items = await Items.GetItems(groupId, album).ConfigureAwait(false) as List<T>;
                    break;

                case ("Member"):
                    items = await Members.GetItems(groupId, album).ConfigureAwait(false) as List<T>;
                    break;

                case ("Image"):
                    items = await Images.GetItems(groupId, album).ConfigureAwait(false) as List<T>;
                    break;

                case ("News"):
                    items = await News.GetItems(groupId).ConfigureAwait(false) as List<T>;
                    break;

                default:
                    throw new Exception("Тип [" + typeof(T).Name + "] не найден");
            }


            await ApplyFavorites(groupId, items).ConfigureAwait(false);

            return items;
        }

        public static async Task<FixedPost> GetFixedPost(int groupId)
        {
            List<FixedPost> posts = await FixedPost.GetItems(groupId).ConfigureAwait(false);
            return posts.Count > 0 ? posts[0] : null; 
        }

        public static async Task<List<IItem>> SearchItems(int groupId, String text)
        {
            List<IItem> items = new List<IItem>();

            items.AddRange(await Members.SearchItems(groupId, text).ConfigureAwait(false));
            items.AddRange(await News.SearchItems(groupId, text).ConfigureAwait(false));
            items.AddRange(await Items.SearchItems(groupId, text).ConfigureAwait(false));
                
            await ApplyFavorites(groupId, items).ConfigureAwait(false);

            return items;
        }

        public static async Task<List<Album>> GetAlbums(int groupId)
        {
            return await Albums.GetAlbums(groupId).ConfigureAwait(false);
        }

        public static async Task<Group> GetGroup(int groupId)
        {
            return await Groups.GetGroup(groupId).ConfigureAwait(false);
        }

        public static async Task<List<Group>> GetGroups()
        {
            return await Groups.GetGroups().ConfigureAwait(false);
        }

        public static async Task UpdateGroup(int groupId, CancellationToken cancellationToken)
        {
            Group group = await RemoteFunctions.GetGroup(groupId, cancellationToken).ConfigureAwait(false);
            await Groups.PutGroup(group).ConfigureAwait(false);
        }

        public static async Task UpdateNews(int groupId, CancellationToken cancellationToken)
        {
            List<News> news = await RemoteFunctions.GetNews(groupId, cancellationToken).ConfigureAwait(false);
            await News.Clear(groupId).ConfigureAwait(false);
            await News.PutItems(news).ConfigureAwait(false);
        }

        public static async Task<News> GetNews(int groupId, int newsId)
        {
            return await News.GetItem(groupId, new Album { Id = newsAlbumId }, newsId).ConfigureAwait(false);
        }

        public static async Task LoadGroup(int groupId, CancellationToken cancellationToken)
        {
            // Альбомы
            List<Album> albums = await RemoteFunctions.GetMenu(groupId, cancellationToken).ConfigureAwait(false);

            // Закрепленный пост
            News fixedPost = await RemoteFunctions.GetFixedPost(groupId, cancellationToken).ConfigureAwait(false);
          
            // Содержимое альбомов
            Dictionary<Album, IList> albumData = new Dictionary<Album, IList>();

            // Содержимое каждого альбома
            foreach (Album album in albums)
            {
                switch (album.Class)
                {
                    case ("Item"):
                        albumData[album] = await RemoteFunctions.GetItems<Item>(groupId, album, cancellationToken).ConfigureAwait(false);
                        break;

                    case ("Member"):
                        albumData[album] = await RemoteFunctions.GetItems<Member>(groupId, album, cancellationToken).ConfigureAwait(false);
                        break;

                    case ("Image"):
                        albumData[album] = await RemoteFunctions.GetItems<Models.Image>(groupId, album, cancellationToken).ConfigureAwait(false);
                        break;

                    default:
                        throw new Exception("Тип [" + album.Class + "] не найден");
                }
            }
 
            // Новости
            await UpdateNews(groupId, cancellationToken).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            // Очистка всего, если удалось получить меню
            await Clear(groupId).ConfigureAwait(false);

            // Заполнение данных
            await Albums.PutAlbums(albums).ConfigureAwait(false);

            await FixedPost.PutItem(fixedPost).ConfigureAwait(false);

            // Содержимое каждого альбома
            foreach (Album album in albums)
            {
                switch (album.Class)
                {
                    case ("Item"):
                        await Items.PutItems((List<Item>)albumData[album]).ConfigureAwait(false);
                        break;

                    case ("Member"):
                        await Members.PutItems((List<Member>)albumData[album]).ConfigureAwait(false);
                        break;

                    case ("Image"):
                        await Images.PutItems((List<Models.Image>)albumData[album]).ConfigureAwait(false);
                        break;

                    default:
                        throw new Exception("Тип [" + album.Class + "] не найден");
                }
            }
        }

        public static async Task CacheImages(int groupId, ObservableProgress progress, CancellationToken cancellationToken) 
        {
            await ImageStore.StroreImageCache(
                await GetImageUrisForGroup(groupId, cancellationToken).ConfigureAwait(false),
                progress,
                cancellationToken
            ).ConfigureAwait(false);
        }

        public static async Task DeleteImages(int groupId, ObservableProgress progress, CancellationToken cancellationToken)
        {
            await ImageStore.ClearImageCache(
                await GetImageUrisForGroup(groupId, cancellationToken).ConfigureAwait(false), 
                progress, 
                cancellationToken
            ).ConfigureAwait(false);
        }

        private static async Task<List<Uri>> GetImageUrisForGroup(int groupId, CancellationToken cancellationToken) 
        {
            List<Uri> uris = new List<Uri>();

            foreach (Item item in await Items.GetItems(groupId).ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();
                await item.FinalizeLoad(uris, cancellationToken).ConfigureAwait(false);
            }

            foreach (Member item in await Members.GetItems(groupId).ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();
                await item.FinalizeLoad(uris, cancellationToken).ConfigureAwait(false);
            }

            foreach (Models.Image item in await Images.GetItems(groupId).ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();
                await item.FinalizeLoad(uris, cancellationToken).ConfigureAwait(false);
            }

            foreach (News item in await News.GetItems(groupId).ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();
                await item.FinalizeLoad(uris, cancellationToken).ConfigureAwait(false);
            }

            return uris;
        }

        public static async Task<Config> GetConfig(int groupId)
        {
            return await Config.GetConfig(groupId).ConfigureAwait(false);
        }

        public static async Task<List<Config>> GetConfigs()
        {
            return await Config.GetConfigs().ConfigureAwait(false);
        }

        public static async Task SaveConfig(Config config)
        {
            await Config.SaveConfig(config).ConfigureAwait(false);
        }

        public static async Task<int> LoadCurrentGroup() 
        {
            if (!await Store.GetFileExistsAsync(CurrentGroupConfigFile).ConfigureAwait(false))
                return 0;

            Stream readStream = await Store.OpenFileAsync(CurrentGroupConfigFile, Xamarin.Forms.Internals.FileMode.Open, Xamarin.Forms.Internals.FileAccess.Read).ConfigureAwait(false);

            StreamReader sr = new StreamReader(readStream);

            String s = await sr.ReadToEndAsync().ConfigureAwait(false);

            sr.Dispose();

            if (readStream != null)
                readStream.Dispose();

            return int.TryParse(s, out int result) ? result: 0;
        }

        public static async Task SaveCurrentGroup(int GroupId)
        {
            Stream writeStream = await Store.OpenFileAsync(CurrentGroupConfigFile, Xamarin.Forms.Internals.FileMode.Create, Xamarin.Forms.Internals.FileAccess.Write).ConfigureAwait(false);

            StreamWriter sw = new StreamWriter(writeStream);

            await sw.WriteAsync(GroupId.ToString()).ConfigureAwait(false);

            sw.Dispose();

            if (writeStream != null)
                writeStream.Dispose();
        }


        private static async Task ApplyFavorites<T>(int groupId, List<T> items) where T : IItem 
        {
            // Заполнение избранного
            List<Favorite> favorites = await Favorites.GetFavorites(groupId).ConfigureAwait(false);
                    
            foreach (T item in items)
            {
                foreach (Favorite favorite in favorites)
                {
                    if (item.IsFavoriteForThis(favorite))
                    {
                        item.SetFavorite(favorite);
                        break;
                    }
                }
            }
        }

        private static String GetDBPath()
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, localDB);

        }
    }
}

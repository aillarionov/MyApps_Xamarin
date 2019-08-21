using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Informer.Models;
using Xamarin.Forms;

namespace Informer.Services.RPC
{
    public static class RemoteFunctions
    {
        private static Connector connector = new Connector();

        public async static Task<List<SimpleGroup>> GetGroups(CancellationToken cancellationToken)
        {
            return await connector.Execute<List<SimpleGroup>>(-1, "GetGroups", null, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<Group> GetGroup(int group, CancellationToken cancellationToken)
        {
            return await connector.Execute<Group>(group, "GetGroup", null, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<String> GetLastChange(int group, CancellationToken cancellationToken)
        {
            return await connector.Execute<String>(group, "GetLastChange", null, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<List<Album>> GetMenu(int group, CancellationToken cancellationToken) {
            return await connector.Execute<List<Album>>(group, "GetMenu", null, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<List<T>> GetItems<T>(int group, Album album, CancellationToken cancellationToken)
        {
            return await connector.Execute<List<T>>(group, "GetItems", new Dictionary<String, int>() { { "albumId", album.Id } }, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<List<News>> GetNews(int group, CancellationToken cancellationToken)
        {
            return await connector.Execute<List<News>>(group, "GetNews", null, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<FixedPost> GetFixedPost(int group, CancellationToken cancellationToken)
        {
            return await connector.Execute<FixedPost>(group, "GetFixedPost", null, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<Stream> LoadPhoto(Uri uri, CancellationToken cancellationToken) {
            return await connector.LoadBinary(uri, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<bool> SendRequest(int group, String name, String company, String phone, String email, String text, CancellationToken cancellationToken)
        {
            Dictionary<String, String> data = new Dictionary<String, String>();
            data.Add("name", name);
            data.Add("company", company);
            data.Add("phone", phone);
            data.Add("email", email);
            data.Add("text", text);

            return await connector.Execute<bool>(group, "SendRequest", data, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<bool> SendQuestion(int group, String name, String phone, String email, String text, CancellationToken cancellationToken)
        {
            Dictionary<String, String> data = new Dictionary<String, String>();
            data.Add("name", name);
            data.Add("phone", phone);
            data.Add("email", email);
            data.Add("text", text);

            return await connector.Execute<bool>(group, "SendQuestion", data, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<byte[]> GetTicket(int group, CancellationToken cancellationToken)
        {
            return await connector.ExecuteBinary<String>(group, "GetTicket", null, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<bool> SendPushToken(List<Config> configs, String token, CancellationToken cancellationToken)
        {
            Dictionary<String, Object> data = new Dictionary<String, Object>();

            data.Add("token", token);
            data.Add("platform", Device.RuntimePlatform);

            List<Dictionary<String, String>> configsData = new List<Dictionary<String, String>>();

            foreach (Config config in configs)
            {
                Dictionary<String, String> tokenData = new Dictionary<String, String>();

                tokenData.Add("group", config.GroupId.ToString());
                tokenData.Add("visitor", config.IsVisitor ? "1" : "0");
                tokenData.Add("presenter", config.IsPresenter ? "1" : "0");

                configsData.Add(tokenData);
            }

            data.Add("configs", configsData);

            return await connector.Execute<bool>(-1, "SendPushToken", data, cancellationToken).ConfigureAwait(false);
        }

        public async static Task<bool> SendAd(List<Config> configs, String adId, CancellationToken cancellationToken)
        {
            Dictionary<String, Object> data = new Dictionary<String, Object>();

            data.Add("platform", Device.RuntimePlatform);
            data.Add("adid", adId);

            List<Dictionary<String, String>> configsData = new List<Dictionary<String, String>>();

            foreach (Config config in configs)
            {
                Dictionary<String, String> tokenData = new Dictionary<String, String>();

                tokenData.Add("group", config.GroupId.ToString());
                tokenData.Add("visitor", config.IsVisitor ? "1" : "0");
                tokenData.Add("presenter", config.IsPresenter ? "1" : "0");

                configsData.Add(tokenData);
            }

            data.Add("configs", configsData);

            return await connector.Execute<bool>(-1, "SendAd", data, cancellationToken).ConfigureAwait(false);
        }
    }
}

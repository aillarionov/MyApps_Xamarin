using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Informer.Services.RPC
{
    public class Connector
    {
        readonly HttpClient client;

        protected internal Connector()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            client = new HttpClient(handler)
            {
                BaseAddress = new Uri(App.BackendUrl),
                Timeout = App.ConnectionTimeout
            };
        }

        public async Task<T> Execute<T>(int group, String method, object param, CancellationToken cancellationToken)
        {

            Dictionary<String, object> request = new Dictionary<String, object>
            {
                { "group", group },
                { "method", method },
                { "params", param }
            };

            String data = JsonConvert.SerializeObject(request);

            HttpResponseMessage httpResponse = await client.PostAsync("", new StringContent(data, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false);

            String response = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!httpResponse.StatusCode.Equals(HttpStatusCode.OK))
            {
                throw new Exception(response);
            }

            return JsonConvert.DeserializeObject<T>(response);
        }

        public async Task<byte[]> ExecuteBinary<T>(int group, String method, object param, CancellationToken cancellationToken)
        {

            Dictionary<String, object> request = new Dictionary<String, object>
            {
                { "group", group },
                { "method", method },
                { "params", param }
            };

            String data = JsonConvert.SerializeObject(request);

            HttpResponseMessage httpResponse = await client.PostAsync("", new StringContent(data, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false);

            byte[] response = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            if (!httpResponse.StatusCode.Equals(HttpStatusCode.OK))
            {
                throw new Exception(Encoding.UTF8.GetString(response));
            }

            return response;
        }

        public async Task<Stream> LoadBinary(Uri uri, CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponse = await client.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            return await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }
    }
}

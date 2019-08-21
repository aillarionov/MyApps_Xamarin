using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Informer.Services.RPC;
using Xamarin.Forms;

namespace Informer.Models
{
    public class Photo
    {
        private int maxSize;
        public Dictionary<int, string> Images { get; set; }

        private Uri _Uri;

        public Photo() {
            this.maxSize = App.maxImageSize;
        }

        public async Task FinalizeLoad(List<Uri> uris, CancellationToken cancellationToken) 
        {
            await CacheImage(uris, cancellationToken).ConfigureAwait(false);
        }

        public Uri Uri 
        {
            get 
            {
                if (this._Uri == null) {
                    GenerateUri();
                }
                return this._Uri;
            }
        }

        public String Serialize() 
        {
            return this.Uri.AbsoluteUri;
        }

        public void Deserialize(String data)
        {
            this._Uri = new Uri(data);
        }

        protected void GenerateUri()
        {
            List<int> sizes = new List<int>();
            int cSize = 0;

            foreach (KeyValuePair<int, string> kvp in this.Images)
            {
                if (kvp.Key >= this.maxSize)
                {
                    sizes.Add(kvp.Key);
                }

                if (kvp.Key >= cSize) 
                {
                    cSize = kvp.Key;
                }
            }

            if (sizes.Count > 0)
            {
                sizes.Sort();
                this._Uri = new Uri(this.Images[sizes[0]]);
            }
            else if (cSize > 0)
            {
                this._Uri = new Uri(this.Images[cSize]);
            }
        }

        private async Task CacheImage(List<Uri> uris, CancellationToken cancellationToken)
        {
            await Task.Run(() => uris.Add(Uri), cancellationToken).ConfigureAwait(false);
        }
    }
}

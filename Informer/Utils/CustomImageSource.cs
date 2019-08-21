using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms
{
    public sealed class CustomImageSource : ImageSource
    {
        internal const string CacheName = "ImageLoaderCache";

        public static readonly BindableProperty DataProperty = BindableProperty.Create("Data", typeof(byte[]), typeof(CustomImageSource), null);

        static CustomImageSource()
        {
        }

        public byte[] Data
        {
            get { return (byte[])GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<Stream> GetStreamAsync(CancellationToken userToken = default(CancellationToken))
        {
            OnLoadingStarted();
            userToken.Register(CancellationTokenSource.Cancel);
            Stream stream;

            try
            {
                stream = await GetStreamAsync(Data, CancellationTokenSource.Token);
                OnLoadingCompleted(false);
            }
            catch (OperationCanceledException)
            {
                OnLoadingCompleted(true);
                throw;
            }
            catch (Exception ex)
            {
                Log.Warning("Image Loading", $"Error getting stream for {Data}: {ex}");
                throw;
            }

            return stream;
        }

        public override string ToString()
        {
            return $"DataLength: {Data.Length}";
        }

        async Task<Stream> GetStreamAsync(byte[] data, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Stream stream;

            stream = await Task.Run(() => { return new MemoryStream(data); });

            return stream;
        }

        void OnDataChanged()
        {
            if (CancellationTokenSource != null)
                CancellationTokenSource.Cancel();
            OnSourceChanged();
        }

    }
}
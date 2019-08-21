using System;
using System.IO;
using System.Threading.Tasks;
using Informer.Utils;
using Java.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Informer.Droid.AndroidPhoto))]
namespace Informer.Droid
{
    public class AndroidPhoto: Java.Lang.Object, IPhoto
    {
        public async Task<Stream> GetPhoto(String path)
        {
            // Open the photo and put it in a Stream to return       
            var memoryStream = new MemoryStream();

            using (var source = new FileInputStream(new Java.IO.File(path)))
            {
                await source.CopyToAsync(memoryStream);
            }

            return memoryStream;
        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;

namespace Informer.Utils
{
    public interface IPhoto
    {
        Task<Stream> GetPhoto(String path);
    }
}

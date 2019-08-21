using System.Collections.Generic;
using System.Threading.Tasks;
using Informer.Models;

namespace Informer.Services
{
    public interface IDataStore<T>
    {
        Task<List<T>> GetItems(int groupId);
        Task<List<T>> GetItems(int groupId, Album album);
    }
}

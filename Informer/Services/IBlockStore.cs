using System.Collections.Generic;
using System.Threading.Tasks;

namespace Informer
{
    public interface IBlockStore<T>
    {
        Task<bool> AddBlockAsync(T block);
        Task<bool> UpdateBlockAsync(T block);
        Task<bool> DeleteBlockAsync(int id);
        Task<T> GetBlockAsync(int id);
        Task<IEnumerable<T>> GetBlocksAsync(bool forceRefresh = false);
    }
}

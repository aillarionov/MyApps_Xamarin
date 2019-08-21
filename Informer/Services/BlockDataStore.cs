using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Informer.Models;

namespace Informer
{
    public class BlockDataStore : IBlockStore<Block>
    {
        List<Block> blocks;

        public BlockDataStore()
        {
            blocks = new List<Block>();
            var mockBlocks = new List<Block>
            {
                new Block { Id = 1, Text = "First block"},
                new Block { Id = 2, Text = "Second block"},
                new Block { Id = 3, Text = "Third block"},
                new Block { Id = 4, Text = "Fourth block"},
                new Block { Id = 5, Text = "Fifth block"},
                new Block { Id = 6, Text = "Sixth block"},
            };

            foreach (var block in mockBlocks)
            {
                blocks.Add(block);
            }
        }

        public async Task<bool> AddBlockAsync(Block block)
        {
            blocks.Add(block);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateBlockAsync(Block block)
        {
            var _block = blocks.Where((Block arg) => arg.Id == block.Id).FirstOrDefault();
            blocks.Remove(_block);
            blocks.Add(block);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteBlockAsync(int id)
        {
            var _block = blocks.Where((Block arg) => arg.Id == id).FirstOrDefault();
            blocks.Remove(_block);

            return await Task.FromResult(true);
        }

        public async Task<Block> GetBlockAsync(int id)
        {
            return await Task.FromResult(blocks.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Block>> GetBlocksAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(blocks);
        }
    }
}

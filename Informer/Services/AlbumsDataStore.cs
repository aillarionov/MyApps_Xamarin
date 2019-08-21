using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services.RPC;

namespace Informer
{
    public class AlbumsDataStore : IDataStore<Album>
    {
     
        public AlbumsDataStore()
        {
        }

        public List<Album> GetItems(bool forceRefresh = false)
        {
            int group_id = 154166391;
            
            var items = RemoteFunctions.GetMenu(group_id).Result;

            return items;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Informer.Models;
using Informer.Services.RPC;

namespace Informer.Services
{
    public class ItemsDataStore1<T> : IDataStore<T>
    {
        private int groupId;

        public ItemsDataStore1(int groupId)
        {
            this.groupId = groupId;
        }

        public List<T> GetItems(Album album, bool forceRefresh = false)
        {

            //Album a = new Album();
            //a.Id = 248227352;
            //a.Id = 248227357;


            var items = RemoteFunctions.GetItems<T>(groupId, album).Result;

            foreach (IItem item in items) 
            {
                item.FinalizeLoad();
            }

            if (typeof(T) == typeof(Item)) 
            {
                Item t = items[0] as Item;
            
                foreach (IItem item in items) 
                {
                    t.Photos.Add((item as Item).Photo);
                }
            }
            return items;
        }
    }
}

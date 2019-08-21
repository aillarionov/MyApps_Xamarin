using System;
using SQLite;

namespace Informer.Models
{
    public class Favorite
    {
        [PrimaryKey]
        #pragma warning disable RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
        public string Id { get { return GroupId.ToString() + "|" + AlbumId.ToString() + "|" + ItemId.ToString(); }  set {; } }
        #pragma warning restore RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter

        public int GroupId { get; set; }
        public int AlbumId { get; set; }
        public int ItemId { get; set; }

        public static Favorite FromItem(Item item) 
        {
            return new Favorite 
            {
                GroupId = item.GroupId,
                AlbumId = item.AlbumId,
                ItemId = item.Id
            };    
        }

        public static bool IsFavoriteForItem(Favorite favorite, Item item) 
        {
            return favorite.GroupId == item.GroupId && favorite.AlbumId == item.AlbumId && favorite.ItemId == item.Id; 
        }
    }
}

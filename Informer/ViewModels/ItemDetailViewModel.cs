using System;
using Informer.Models;

namespace Informer.ViewModels
{
    public class ItemDetailViewModel<T> : BaseViewModel where T : IItem
    {
        public T Item { get; set; }
        public ItemDetailViewModel(T item) 
        {
            Item = item;
        }
    }
}

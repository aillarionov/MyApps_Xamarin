using System;
using System.Collections.Generic;

namespace Informer.Models
{
    public class Block
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Photo Photo { get; set; };
        public List<Photo> Photos { get; set; }
    }
}

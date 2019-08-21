using System;
namespace Informer.Models
{
    public class Album
    {
        public int GroupId { get; set; }
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int Size { get; set; }
        public string Class { get; set; }
        public string Icon { get; set; }
    }
}
